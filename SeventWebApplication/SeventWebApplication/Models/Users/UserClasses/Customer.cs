namespace SeventWebApplication.Models.Users
{
    // Kunden får bara kolla på sina egna bokningar.
    // (Skapa bokningar fixas i bokningsdelen.)
    public class Customer : User, IBookingViewer
    {
        public override Role Role => Role.Customer;

        // Filtrerar bort allt som inte är kundens
        public IBookingVisibility Visibility { get; }

        public Customer(Guid userId, string name, string phone, string mail, string password, bool isActive = true)
            : base(userId, name, phone, mail, password, isActive)
        {
            Visibility = new OwnBookingVisibility(userId);
        }
    }
}
