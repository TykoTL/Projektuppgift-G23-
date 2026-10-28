using BookingModel = SeventWebApplication.Models.Booking.Booking;

namespace SeventWebApplication.Models.Users
{
    // Visar bokningar för en användare.
    // Hämtar alla bokningar från FleetManager och låter användarens egen Visibility strategi
    // abgöra vad den får se. Ingen if-sats på roller behövs, och lägger vi till nya roller
    // behöver vi inte röra den här klassen (OCP).
    public class BookingViewerService
    {
        public IEnumerable<BookingModel> ViewBookings(IBookingViewer viewer) //endast de som implementerar IBookingViewer kan
                                                                             //anropa den här metoden, aka
                                                                             //Admin, Employee och Customer. (ISP)
        {
            ArgumentNullException.ThrowIfNull(viewer);

            // Inaktiverade konton får inte se nåt
            if (!viewer.IsActive)
                return Enumerable.Empty<BookingModel>();

            // FleetManager ligger utan namespace i repot, därför global::
            IEnumerable<BookingModel> allBookings = global::FleetManager.instance.GetBookings();

            return viewer.Visibility.Filter(allBookings);
        }
    }
}
