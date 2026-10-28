namespace SeventWebApplication.Models.Users
{
    // Det minsta man behöver veta om den som gör nåt aka vem det är och om kontot är aktivt.
    // Tjänsterna behöver inte hela User-objektet, så de tar bara in det här (ISP, DIP).
    public interface IActor
    {
        Guid UserId { get; }
        bool IsActive { get; }
    }
}
