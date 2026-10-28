namespace SeventWebApplication.Models.Users
{
    // "Den här användaren får hantera konton." Bara Admin har den här,
    // så kompilatorn stoppar om man försöker skicka in en Employee till AccountService.
    public interface IAccountManager : IActor
    {
    }
}
