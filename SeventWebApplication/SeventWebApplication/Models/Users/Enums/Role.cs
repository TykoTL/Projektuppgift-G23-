namespace SeventWebApplication.Models.Users
{
    // Rollerna som finns. UserFactory kollar på den här för att veta vilken
    // klass den ska skapa, och AccountService använder den när admin byter roll på nån
    // eller skapar ett nytt konto.
    public enum Role
    {
        Customer,
        Employee,
        Admin
    }
}
