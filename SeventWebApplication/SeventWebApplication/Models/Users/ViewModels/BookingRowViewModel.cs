using SeventWebApplication.Models.Addon;
using BookingModel = SeventWebApplication.Models.Booking.Booking;

namespace SeventWebApplication.Models.Users
{
    public enum BookingTimeStatus
    {
        Pending,
        Confirmed,
        Active,
        Completed,
        Cancelled
    }

    // Status är ju inte implementerat än men lägger som enum här för att det ska vara lättare att implementera senare.
    // Då kan man bara ändra From-metoden och vyn behöver inte ändras. För tillfället ger vi bara bokningarna random status vid seedning

    // En rad i boknings-tabellen. Vyn ska inte behöva gräva i Booking/Vehicle/Addon
    // själv, så vi plockar ihop allt den behöver här.
    public class BookingRowViewModel
    {
        public string BookingId { get; init; } = "";
        public string ShortId => BookingId.Length > 8 ? BookingId[..8].ToUpper() : BookingId.ToUpper();

        public string CustomerName { get; init; } = "";
        public string CustomerMail { get; init; } = "";

        public string Vehicle { get; init; } = "";
        public string RegNr { get; init; } = "";

        public DateTime Start { get; init; }
        public DateTime End { get; init; }
        public int Days => Math.Max(1, (int)Math.Ceiling((End - Start).TotalDays)); // så att en bokning som startar och slutar samma dag räknas som 1 dag

        public List<string> Addons { get; init; } = new();
        public decimal? TotalPrice { get; init; }

        public BookingTimeStatus Status { get; init; }

        public static BookingRowViewModel From(BookingModel booking, IUserRepository users, DateTime now)
        {
            // Booking.UserId är en string, så vi får parsa den för att hitta kunden
            User? customer = Guid.TryParse(booking.UserId, out Guid id) ? users.GetById(id) : null;

            BookingTimeStatus status = booking.Status?.Trim() switch
            {
                "Pending" => BookingTimeStatus.Pending,
                "Confirmed" => BookingTimeStatus.Confirmed,
                "Active" => BookingTimeStatus.Active,
                "Completed" => BookingTimeStatus.Completed,
                "Cancelled" => BookingTimeStatus.Cancelled,
                _ => BookingTimeStatus.Pending
            };

            return new BookingRowViewModel
            {
                BookingId = booking.BookingId,
                CustomerName = customer?.Name ?? "Okänd kund",
                CustomerMail = customer?.Mail ?? "",
                Vehicle = booking.Vehicle is null
                    ? "–"
                    : $"{booking.Vehicle.Model.Brand.BrandName} {booking.Vehicle.Model.ModelName}",
                RegNr = booking.Vehicle?.RegistrationPlate.RegNr ?? "",
                Start = booking.StartDate,
                End = booking.EndDate,
                Addons = GetAddonNames(booking.Addon),
                TotalPrice = booking.Addon?.GetPrice(),
                Status = status
            };
        }

        // Går igenom Decorator-kedjan lager för lager och samlar ihop namnen på tillvalen.
        // T.ex. ChildSeat(GPS(VehicleBase)) blir ["Barnstol", "GPS"].
        private static List<string> GetAddonNames(IAddon? addon)
        {
            var names = new List<string>();

            while (addon is AddonDecorator decorator)
            {
                names.Add(decorator switch
                {
                    GPSAddon => "GPS",
                    ChildSeatAddon => "Barnstol",
                    ExtraDriverAddon => "Extra förare",
                    _ => decorator.GetType().Name   // nya tillval syns ändå, bara med klassnamnet
                });

                addon = decorator.GetWrappedAddon();
            }

            names.Reverse();   // så tillvalen kommer i ordningen de lades till

            // Slår ihop dubbletter, så tre barnstolar blir "Barnstol ×3" istället för tre badges
            return names
                .GroupBy(n => n)
                .Select(g => g.Count() > 1 ? $"{g.Key} ×{g.Count()}" : g.Key)
                .ToList();
        }
    }
}
