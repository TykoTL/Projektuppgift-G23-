namespace SeventWebApplication.Models.Users
{
    // Kontohantering, bara för admins.
    // Eftersom alla metoder tar in IAccountManager går det inte att skicka in
    // en Customer eller Employee, kompilatorn säger nej direkt. Endast de som implementerar IAccountManager
    // kan anropa metoderna, vilket endast är Admin i detta fall. (ISP)
    public class AccountService
    {
        private readonly IUserRepository _users;

        public AccountService(IUserRepository users)
        {
            _users = users;
        }

        public void Deactivate(IAccountManager actor, Guid userId)
        {
            User target = GetTargetForChange(actor, userId);
            target.Deactivate();
            _users.Update(target);
        }

        public void Reactivate(IAccountManager actor, Guid userId)
        {
            EnsureActorIsValid(actor);
            User target = GetUser(userId);
            target.Reactivate();
            _users.Update(target);
        }

        // För att byta roll gör UserFactory ett nytt objekt av rätt klass med samma id,
        // och sen byter vi ut det gamla i repot.
        // Kan bara byta mellan Customer och Employee, admin-kontot är förseedat och man kan inte skapa fler admin-konton i detta fall, men vill vi
        // kunna göra de är de bara att lägga till Admin i AssignableRoles och ändra UserFactory.Create så den kan skapa Admin också.
        public User ChangeRole(IAccountManager actor, Guid userId, Role newRole)
        {
            EnsureAssignableRole(newRole);

            User target = GetTargetForChange(actor, userId);

            if (target is IAccountManager)
                throw new InvalidOperationException("Admin-kontot kan inte byta roll.");

            if (target.Role == newRole)
                return target;   // redan rätt roll, inget att göra

            User updated = UserFactory.ChangeRole(target, newRole);
            _users.Update(updated);
            return updated;
        }

        public IEnumerable<User> GetAllUsers(IAccountManager actor)
        {
            EnsureActorIsValid(actor);
            return _users.GetAll();
        }

        // Admin skapar ett nytt konto, antingen kund eller anställd.
        // Funkar som Register i AuthService fast admin väljer rollen.
        // Samma här att om vi vill kunna regga nya admins så är det bara att lägga till
        // Admin i AssignableRoles och ändra UserFactory.Create så borde det funka ez clap.
        public User CreateUser(IAccountManager actor, Role role, string name, string mail, string phone, string password)
        {
            EnsureActorIsValid(actor);
            EnsureAssignableRole(role);

            // Samma lösenordskrav som när man registrerar sig själv
            if (string.IsNullOrWhiteSpace(password) || password.Length < 8)
                throw new ArgumentException("Lösenordet måste vara minst 8 tecken.", nameof(password));

            if (_users.GetByMail(mail) is not null)
                throw new InvalidOperationException("Det finns redan ett konto med den e-postadressen.");

            User user = UserFactory.Create(role, Guid.NewGuid(), name, phone, mail, password);
            _users.Add(user);
            return user;
        }

        // Ändra namn och telefon. Mailen går inte att ändra eftersom det är den man loggar in med.
        // Här får admin ändra sitt eget konto också, det kan man inte låsa ute sig med.
        public void UpdateContactInfo(IAccountManager actor, Guid userId, string name, string phone)
        {
            EnsureActorIsValid(actor);
            User target = GetUser(userId);
            target.UpdateContactInfo(name, phone);
            _users.Update(target);
        }



        // Dubbelkollar mot repot att admin fortfarande finns, är aktiv och faktiskt är admin.
        // detta förhindrar att nnågon inte får göra det dom inte ska kunna göra ifall deras roll ändras under runtime

        private void EnsureActorIsValid(IAccountManager actor)
        {
            ArgumentNullException.ThrowIfNull(actor);

            User? current = _users.GetById(actor.UserId);
            if (current is not IAccountManager || !current.IsActive)
                throw new UnauthorizedAccessException("Du har inte behörighet att administrera konton.");
        }

        // Rollerna admin får dela ut. Admin är inte med, det finns bara det förseedade kontot, men kan ändras om man vill.
        public static readonly IReadOnlyList<Role> AssignableRoles = new[] { Role.Customer, Role.Employee };

        private static void EnsureAssignableRole(Role role)
        {
            if (!AssignableRoles.Contains(role))
                throw new InvalidOperationException("Man kan bara välja Kund eller Anställd.");
        }

        private User GetUser(Guid userId)
        {
            return _users.GetById(userId)
                ?? throw new InvalidOperationException("Användaren hittades inte.");
        }

        // Admin får inte inaktivera eller byta roll på sig själv,
        // annars kan man råka låsa ute den enda admin som finns.
        private User GetTargetForChange(IAccountManager actor, Guid userId)
        {
            EnsureActorIsValid(actor);

            if (actor.UserId == userId)
                throw new InvalidOperationException("Du kan inte ändra ditt eget konto på det här sättet.");

            return GetUser(userId);
        }
    }
}
