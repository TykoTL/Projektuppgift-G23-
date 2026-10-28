namespace SeventWebApplication.Models.Users
{
    // Håller koll på vem som är inloggad.
    // ASP.NET gör nya objekt vid varje sidladdning, så ett vanligt fält
    // skulle glömma bort inloggningen direkt. Därför sparar vi användarens id i
    // ASP.NET:s session istället (den kopplas till webbläsaren med en cookie).
    // Vi hämtar sen User-objektet från repot varje gång, så om admin byter roll
    // eller inaktiverar nån syns det direkt.
    public class UserSession
    {
        private const string SessionKey = "CurrentUserId";

        private readonly AuthService _auth;
        private readonly IUserRepository _users;
        private readonly IHttpContextAccessor _http;

        public UserSession(AuthService auth, IUserRepository users, IHttpContextAccessor http)
        {
            _auth = auth;
            _users = users;
            _http = http;
        }

        // Sessionen för anropet som pågår just nu
        private ISession Session =>
            _http.HttpContext?.Session
            ?? throw new InvalidOperationException("Ingen aktiv HTTP-session.");

        // Null om ingen är inloggad, eller om kontot har blivit inaktiverat
        public User? CurrentUser
        {
            get
            {
                string? id = Session.GetString(SessionKey);
                if (!Guid.TryParse(id, out Guid userId))
                    return null;

                User? user = _users.GetById(userId);
                return user is { IsActive: true } ? user : null;
            }
        }

        public bool IsLoggedIn => CurrentUser is not null;

        public User Login(string mail, string password)
        {
            User user = _auth.Login(mail, password);   // kastar fel om nåt inte stämmer
            Session.SetString(SessionKey, user.UserId.ToString());
            return user;
        }

        public void Logout()
        {
            Session.Remove(SessionKey);
        }
    }
}
