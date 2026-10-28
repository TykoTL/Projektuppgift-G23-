using BookingModel = SeventWebApplication.Models.Booking.Booking;

namespace SeventWebApplication.Models.Users
{
    // Strategy-interfacet tar in alla bokningar och visar endast de man får se.
    // Aliaset bookingmodel behövs för att både namnspace och klassen heter Booking, annars blir C# förvirrad
    public interface IBookingVisibility
    {
        IEnumerable<BookingModel> Filter(IEnumerable<BookingModel> bookings);
    }
}
