namespace SeventWebApplication.Models.Users
{
    // Allt som har med att spara/hämta användare att göra.
    // Tjänsterna pratar bara med interfacet (DIP), så vill vi byta till en riktig
    // databas sen behöver vi bara göra en ny klass, inget annat ändras.
    public interface IUserRepository
    {
        User? GetById(Guid id);
        User? GetByMail(string mail);
        IEnumerable<User> GetAll();
        void Add(User user);
        void Update(User user); // Ett edge case som kan uppstå är när vi uppdaterar en
                                // användare med samma UserId men ny roll så
                                // måste vi ersätta den gamla versionen i repot.
                                // Om den nya e-postadressen redan finns på en annan användare får vi inte
                                // skriva över den, annars kan två användare "sopa ihop" i _usersByMail och
                                // logik/behörigheter blir felaktiga. Måste alltså lägga till nån kontroll i tjänsten som kallar Update()
                                // för att se till att det inte finns en annan användare med samma e-postadress.
                                // Vi kan inte göra den kontrollen här i repot eftersom vi inte har tillgång till alla användare, bara den som uppdateras.
                                // kanske borde kolla så telefonnummer inte är samma också, men det är inte lika viktigt
                                // eftersom vi inte använder telefonnummer som nyckel i repot.
    }
}
