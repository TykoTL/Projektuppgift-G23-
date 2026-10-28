using Microsoft.AspNetCore.Mvc;
using SeventWebApplication.Models.Users;

namespace SeventWebApplication.Controllers
{
    // Sidan där man kollar på bokningar (/Bookings).
    // Controllern bestämmer INTE vilka bokningar som syns – det gör användarens
    // egen Visibility via BookingViewerService. Här fixar vi bara inloggningskollen,
    // filter/sök och gör om bokningarna till rader som vyn kan visa.
    public class BookingsController : Controller
    {
        private readonly UserSession _session;
        private readonly BookingViewerService _viewer;
        private readonly IUserRepository _users;

        public BookingsController(UserSession session, BookingViewerService viewer, IUserRepository users)
        {
            _session = session;
            _viewer = viewer;
            _users = users;
        }

        // status = "pending" / "confirmed" / "active" / "completed" / "cancelled" (eller inget = alla)
        // search = fritext, bara för personal
        [HttpGet]
        public IActionResult Index(string? status, string? search)
        {
            User? user = _session.CurrentUser;

            // Inte inloggad? Skicka till inloggningen och sen tillbaka hit
            if (user is null)
                return RedirectToAction("Login", "Account", new { returnUrl = Url.Action("Index", "Bookings") });

            // Alla roller vi har nu kan titta på bokningar, men om nån roll inte kan det sen
            // så stoppas den här istället för att krascha.
            // (StatusCode(403) istället för Forbid(), Forbid kräver ASP.NET:s inbyggda inloggning som vi inte kör)
            // Detta borde aldrig hända, men det är bra att ha en check här ifall nån roll tas bort eller läggs till i framtiden.
            if (user is not IBookingViewer viewer)
                return StatusCode(403);

            bool isStaff = user is IBookingManager;
            DateTime now = DateTime.Now;

            // Det är här det bestäms vilka bokningar som syns för den här användaren.
            // Vi får tillbaka alla bokningar som användaren kan se i en lista, och gör om dem till rader som sidan kan visa.
            List<BookingRowViewModel> rows = _viewer.ViewBookings(viewer)
                .Select(b => BookingRowViewModel.From(b, _users, now))
                .ToList();

            // Sökfunktion för personal och admins, funkar med kundnamn, mail, regnr, bil eller boknings-id
            string? trimmedSearch = isStaff ? search?.Trim() : null;
            if (!string.IsNullOrEmpty(trimmedSearch))
            {
                rows = rows.Where(r =>
                        r.CustomerName.Contains(trimmedSearch, StringComparison.OrdinalIgnoreCase) ||
                        r.CustomerMail.Contains(trimmedSearch, StringComparison.OrdinalIgnoreCase) ||
                        r.RegNr.Contains(trimmedSearch, StringComparison.OrdinalIgnoreCase) ||
                        r.Vehicle.Contains(trimmedSearch, StringComparison.OrdinalIgnoreCase) ||
                        r.BookingId.Contains(trimmedSearch, StringComparison.OrdinalIgnoreCase))
                    .ToList();
            }

            // Räkna per status INNAN vi filtrerar på status, så flikarna visar rätt siffror
            BookingTimeStatus? selected = ParseStatus(status);

            var model = new BookingListViewModel
            {
                IsStaff = isStaff,
                Search = trimmedSearch,
                SelectedStatus = selected,
                CountAll = rows.Count,
                CountPending = rows.Count(r => r.Status == BookingTimeStatus.Pending),
                CountConfirmed = rows.Count(r => r.Status == BookingTimeStatus.Confirmed),
                CountActive = rows.Count(r => r.Status == BookingTimeStatus.Active),
                CountCompleted = rows.Count(r => r.Status == BookingTimeStatus.Completed),
                CountCancelled = rows.Count(r => r.Status == BookingTimeStatus.Cancelled),
                Bookings = rows
                    .Where(r => selected is null || r.Status == selected)
                    .OrderBy(r => (int)r.Status)
                    .ThenBy(r => r.Start)
                    .ToList()
            };

            return View(model);
        }

        // Här hanteras statusfiltren, de gör om texten från URL:en till en status, eller null för inget filter aka visa alla
        private static BookingTimeStatus? ParseStatus(string? status) => status?.ToLowerInvariant() switch
        {
            "pending" => BookingTimeStatus.Pending,
            "confirmed" => BookingTimeStatus.Confirmed,
            "active" => BookingTimeStatus.Active,
            "completed" => BookingTimeStatus.Completed,
            "cancelled" => BookingTimeStatus.Cancelled,
            _ => null
        };
    }
}
