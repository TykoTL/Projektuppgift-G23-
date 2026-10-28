namespace SeventWebApplication.Models.Users
{
    // Det som skickas in från "Skapa användare"-formuläret
    public class CreateUserForm
    {
        public string Name { get; set; } = "";
        public string Mail { get; set; } = "";
        public string Phone { get; set; } = "";
        public string Password { get; set; } = "";
        public Role Role { get; set; } = Role.Customer;
    }

    // Det som skickas in från "Redigera"-formuläret.
    // Ingen Mail här med flit, den går inte att ändra då den används för att logga in
    public class EditUserForm
    {
        public Guid UserId { get; set; }
        public string Name { get; set; } = "";
        public string Phone { get; set; } = "";
        public Role Role { get; set; }
    }

    // Detta representerar en rad i användarlistan
    public class UserRowViewModel
    {
        public User User { get; init; } = null!;
        public int BookingCount { get; init; }
        public bool IsMe { get; init; }   // så vi kan gömma knappar man inte får använda på sig själv
    }

    public class UserListViewModel
    {
        public List<UserRowViewModel> Users { get; init; } = new();
        public string? Search { get; init; }
        public Role? SelectedRole { get; init; }

        // Siffrorna i flikarna (räknas efter sök men innan rollfiltret)
        public int CountAll { get; init; }
        public int CountCustomers { get; init; }
        public int CountEmployees { get; init; }
        public int CountAdmins { get; init; }
    }

    // Redigera-sidan behöver både formuläret och användaren (för mail, status osv)
    public class EditUserViewModel
    {
        public EditUserForm Form { get; init; } = new();
        public User User { get; init; } = null!;
        public bool IsMe { get; init; }

        // Admin-kontot (och en själv) kan inte byta roll, då låser vi rollväljaren
        public bool CanChangeRole => !IsMe && User is not IAccountManager;
    }
}
