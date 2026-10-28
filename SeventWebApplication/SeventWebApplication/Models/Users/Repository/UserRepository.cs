namespace SeventWebApplication.Models.Users
{
    // Sparar användarna i minnet, samma upplägg som VehicleBrandRepository.
    // Använder två dictionaries, en på id och en på mail, så inloggningen (som söker på mail) går snabbt.
    public class UserRepository : IUserRepository
    {
        private readonly Dictionary<Guid, User> _usersById = new();
        private readonly Dictionary<string, User> _usersByMail = new(StringComparer.OrdinalIgnoreCase);

        public User? GetById(Guid id)
        {
            return _usersById.GetValueOrDefault(id);
        }

        public User? GetByMail(string mail)
        {
            if (string.IsNullOrWhiteSpace(mail))
                return null;

            return _usersByMail.GetValueOrDefault(User.NormalizeMail(mail));
        }

        public IEnumerable<User> GetAll()
        {
            return _usersById.Values.ToList().AsReadOnly();
        }

        public void Add(User user)
        {
            ArgumentNullException.ThrowIfNull(user);

            if (_usersByMail.ContainsKey(user.Mail))
                throw new InvalidOperationException("Det finns redan ett konto med den e-postadressen.");

            if (_usersById.ContainsKey(user.UserId))
                throw new InvalidOperationException("Användaren finns redan.");

            _usersById[user.UserId] = user;
            _usersByMail[user.Mail] = user;
        }

        // Byter ut den sparade användaren mot den nya med samma id.
        // Behövs när man byter roll, då är det ju ett helt nytt objekt.
        // Undviker problemet att om den nya e-postadressen redan finns på en annan användare
        // så får vi inte skriva över den i _usersByMail, annars kan två användare
        // överlappa och login/behörighetslogik ballar ur.
        public void Update(User user)
        {
            ArgumentNullException.ThrowIfNull(user);

            if (!_usersById.TryGetValue(user.UserId, out User? old))
                throw new InvalidOperationException("Användaren finns inte.");

            string normalizedNewMail = User.NormalizeMail(user.Mail);
            string normalizedOldMail = User.NormalizeMail(old.Mail);

            if (!string.Equals(normalizedOldMail, normalizedNewMail, StringComparison.OrdinalIgnoreCase)
                && _usersByMail.ContainsKey(normalizedNewMail))
            {
                throw new InvalidOperationException("Det finns redan ett konto med den e-postadressen.");
            }

            _usersByMail.Remove(normalizedOldMail);
            _usersById[user.UserId] = user;
            _usersByMail[normalizedNewMail] = user;
        }

        // Skapar lite test konton
        public void Seed()
        {
            if (_usersById.Count > 0)
                return;

            Add(UserFactory.Create(Role.Admin,    Guid.NewGuid(), "Boss Adminsson",   "070-0000001", "admin@sevent.se",      "password"));
            Add(UserFactory.Create(Role.Employee, Guid.NewGuid(), "Mike Hawk",       "072-1111112", "employee@sevent.se",   "123123123"));
            Add(UserFactory.Create(Role.Customer, Guid.NewGuid(), "Ben Dover",       "076-9999999", "lilben@hotmail.com",   "123123123"));
            Add(UserFactory.Create(Role.Customer, Guid.NewGuid(), "Joe Coxlong",     "073-7777777", "bigjoe@hotmail.com",  "123123123"));
            Add(UserFactory.Create(Role.Customer, Guid.NewGuid(), "Lisa Kund",       "070-2222222", "lisa@sevent.se",       "123123123"));
            Add(UserFactory.Create(Role.Customer, Guid.NewGuid(), "Nina Svensson",   "070-3333333", "nina@sevent.se",       "123123123"));
            Add(UserFactory.Create(Role.Customer, Guid.NewGuid(), "Olle Pettersson", "070-4444444", "olle@sevent.se",      "123123123"));
            Add(UserFactory.Create(Role.Customer, Guid.NewGuid(), "Pia Nyberg",      "070-5555555", "pia@sevent.se",        "123123123"));
        }
    }
}
