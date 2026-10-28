namespace SeventWebApplication.Models.Users
{
    // Anställd ser alla bokningar och får hantera dem. Ingen användarhantering
    public class Employee : User, IBookingViewer, IBookingManager
    {
        public override Role Role => Role.Employee;

        public IBookingVisibility Visibility { get; } = new AllBookingVisibility();

        public Employee(Guid userId, string name, string phone, string mail, string password, bool isActive = true)
            : base(userId, name, phone, mail, password, isActive)
        {
        }
    }
}
