namespace SeventWebApplication.Models.Users
{
    // "Den här användaren får kolla på dessa bokningar."
    // Varje roll har med sig sin egen Visibility som bestämmer VILKA bokningar den ser
    // (Strategy). Då slipper BookingViewerService fråga "vilken roll är du?". Behöver bara kolla på UserId, att kontot är aktivt, och implementerar IBookingViewer, och då returneras automatiskt rätt lista.
    public interface IBookingViewer : IActor
    {
        IBookingVisibility Visibility { get; }
    }
}
