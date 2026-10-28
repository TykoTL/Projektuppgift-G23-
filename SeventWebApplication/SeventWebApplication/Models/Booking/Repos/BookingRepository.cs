namespace SeventWebApplication.Models.Booking
{
    // La tll denna så jag skulle kunna hämta bookings till vehicle assigns
    public class BookingRepository
    {
        private readonly List<Booking> _bookings = new();

        public void Add(Booking booking)
        {
            if (booking == null)
                throw new ArgumentNullException(nameof(booking));

            if (_bookings.Any(b => b.BookingId == booking.BookingId))
                throw new InvalidOperationException("Booking already exists.");

            _bookings.Add(booking);
        }

        public Booking? GetById(string bookingId)
            => _bookings.FirstOrDefault(b => b.BookingId == bookingId);

        public IEnumerable<Booking> GetByUserId(string userId)
            => _bookings.Where(b => b.UserId == userId).ToList();

        public IEnumerable<Booking> GetAll()
            => _bookings.ToList();

        public void Update(Booking booking)
        {
            var index = _bookings.FindIndex(b => b.BookingId == booking.BookingId);
            if (index < 0)
                throw new InvalidOperationException("Booking not found.");

            _bookings[index] = booking;
        }

        public bool Remove(string bookingId)
            => _bookings.RemoveAll(b => b.BookingId == bookingId) > 0;
    }
}