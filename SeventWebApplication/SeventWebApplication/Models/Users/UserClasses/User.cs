namespace SeventWebApplication.Models.Users
{
    // Basklassen för alla användare. Här ligger bara grejer som alla har:
    // id, namn, kontaktinfo, lösenord och om kontot är aktivt.
    // Vad man FÅR göra bestäms inte här, utan av vilka interface subklassen har
    // (IBookingViewer, IBookingManager, IAccountManager).
    public abstract class User : IActor
    {
        public Guid UserId { get; }
        public string Name { get; private set; }
        public string Phone { get; private set; }
        public string Mail { get; private set; }

        // Klartext, palla inte krångla med hashning i det här projektet
        public string Password { get; private set; }

        // Går bara att ändra via Deactivate/Reactivate längre ner
        public bool IsActive { get; private set; }

        // Varje subklass säger själv vilken roll den är
        public abstract Role Role { get; }

        protected User(Guid userId, string name, string phone, string mail, string password, bool isActive)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(name);
            ArgumentException.ThrowIfNullOrWhiteSpace(mail);
            ArgumentException.ThrowIfNullOrWhiteSpace(password);

            if (!mail.Contains('@'))
                throw new ArgumentException("Ogiltig e-postadress.", nameof(mail));

            UserId = userId;
            Name = name.Trim();
            Phone = phone?.Trim() ?? string.Empty;
            Mail = NormalizeMail(mail);
            Password = password;
            IsActive = isActive;
        }

        // internal = ser till att bara koden i samma projekt/assembly kan anropa metoden,
        // vilket i detta fall är accountservice som endast admins kan anropa. (ISP, DIP)
        internal void Deactivate() => IsActive = false;
        internal void Reactivate() => IsActive = true;


        // Stämmer lösenordet?
        public bool CheckPassword(string password) => Password == password;

        public void UpdateContactInfo(string name, string phone)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(name);
            Name = name.Trim();
            Phone = phone?.Trim() ?? string.Empty;
        }

        // Så att den skiter i stora/små bokstäver och mellanslag i början/slutet när man jämför mailadresser
        public static string NormalizeMail(string mail) => mail.Trim().ToLowerInvariant();
    }
}
