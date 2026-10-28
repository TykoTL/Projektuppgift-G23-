namespace SeventWebApplication.Models.Users
{
    // Factoryt är det enda stället som vet vilken klass som hör till vilken roll.
    // Lägger vi till en ny roll är det bara switchen här som behöver ändras. Lätt att lägga till nya roller utan att behöva ändra i tjänsterna som använder User-objektet aka OCP
    public static class UserFactory
    {
        public static User Create(Role role, Guid userId, string name, string phone,
                                  string mail, string password, bool isActive = true)
        {
            return role switch
            {
                Role.Customer => new Customer(userId, name, phone, mail, password, isActive),
                Role.Employee => new Employee(userId, name, phone, mail, password, isActive),
                Role.Admin    => new Admin(userId, name, phone, mail, password, isActive),
                _ => throw new ArgumentOutOfRangeException(nameof(role), role, "Okänd roll.")
            };
        }

        // För rollbyte kan man inte bara typ ändra en Customer till en Employee,
        // utan vi gör ett nytt objekt med samma id och samma info.
        // Bokningarna pekar på UserId, så de hänger med automatiskt.
        // Vi vill inte ändra på det gamla objektet, för då kan nån som har en referens
        // till det fortfarande tro att det är en Customer fast det egentligen är en Employee nu.
        // det gamla objektet 
        public static User ChangeRole(User existing, Role newRole)
        {
            return Create(newRole, existing.UserId, existing.Name, existing.Phone,
                          existing.Mail, existing.Password, existing.IsActive);
        }
    }
}
