namespace SeventWebApplication.Models.Users
{
    // Sköter inloggning och registrering. User-klassen ska inte behöva bry sig om
    // hur inloggning funkar, så det ligger här istället (High Cohesion).
    public class AuthService
    {
        private readonly IUserRepository _users;

        public AuthService(IUserRepository users)
        {
            _users = users;
        }

        // Ger tillbaka användaren om allt stämmer, annars kastas ett fel.
        // Samma felmeddelande oavsett om det är mailen eller lösenordet som är fel,
        // så man inte kan lista ut vilka mailadresser som har konton.
        public User Login(string mail, string password)
        {
            User? user = _users.GetByMail(mail);

            if (user is null || !user.CheckPassword(password))
                throw new InvalidOperationException("Fel e-postadress eller lösenord.");

            if (!user.IsActive)
                throw new InvalidOperationException("Kontot är inaktiverat. Kontakta personalen.");

            return user;
        }

        // Registrerar man sig själv blir man alltid kund.
        // Anställda får skapas av en admin och som vi har det nu finns endast ett seedat admin konto.
        public Customer Register(string name, string mail, string phone, string password)
        {
            if (string.IsNullOrWhiteSpace(password) || password.Length < 8)
                throw new ArgumentException("Lösenordet måste vara minst 8 tecken.", nameof(password));

            if (_users.GetByMail(mail) is not null)
                throw new InvalidOperationException("Det finns redan ett konto med den e-postadressen.");

            var customer = (Customer)UserFactory.Create(
                Role.Customer, Guid.NewGuid(), name, phone, mail, password);

            _users.Add(customer);
            return customer;
        }
    }
}
