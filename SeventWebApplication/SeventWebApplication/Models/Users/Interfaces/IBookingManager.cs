namespace SeventWebApplication.Models.Users
{
    // "Den här användaren får hantera bokningar" (godkänna, avboka osv).
    // Själva logiken ska ligga i bokningsdelen, men interfacet hör hit eftersom det är
    // rollerna som implementerar det. Typ en behörighets check istället för if satser, en metod som tar
    // IBookingManager går inte ens att anropa med en Customer utan som det ser ut nu komemr bara admins och empolyees kunna anropa den. (lite ISP, lite DIP)
    // En boknings hanterare behöver därför inte veta om det är en admin eller employee utan behöver
    // bara kolla UserId, är kontot aktivt, och implementerar det ibookingmanager (annars får man inte anropa metoden).
    public interface IBookingManager : IActor
    {
    }
}
