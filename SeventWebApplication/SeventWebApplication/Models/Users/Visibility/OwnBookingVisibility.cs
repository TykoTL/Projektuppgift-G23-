using BookingModel = SeventWebApplication.Models.Booking.Booking;

namespace SeventWebApplication.Models.Users
{
    // För kunder ser man bara bokningarna som är ens egna.
    public class OwnBookingVisibility : IBookingVisibility
    {
        // Booking.UserId är en string i repot, så vi sparar id:t som string också
        private readonly string _userId;

        public OwnBookingVisibility(Guid userId)
        {
            _userId = userId.ToString();
        }

        public IEnumerable<BookingModel> Filter(IEnumerable<BookingModel> bookings)
        {
            return bookings.Where(b =>
                string.Equals(b.UserId, _userId, StringComparison.OrdinalIgnoreCase));
        }
    }
}
