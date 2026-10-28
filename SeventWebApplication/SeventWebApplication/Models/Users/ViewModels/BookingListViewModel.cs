namespace SeventWebApplication.Models.Users
{
    // Allt boknings-sidan behöver är raderna + lite info om vem som tittar och vilket filter som är valt
    public class BookingListViewModel
    {
        public List<BookingRowViewModel> Bookings { get; init; } = new();

        // Personal ser kundkolumnen och kan söka, kunder ser bara sina egna så det behövs inte
        public bool IsStaff { get; init; }

        public BookingTimeStatus? SelectedStatus { get; init; }
        public string? Search { get; init; }

        // Antal per status, visas i flikarna (räknas innan statusfiltret, annars blir siffrorna fel)
        public int CountAll { get; init; }
        public int CountPending { get; init; }
        public int CountConfirmed { get; init; }
        public int CountActive { get; init; }
        public int CountCompleted { get; init; }
        public int CountCancelled { get; init; }
    }
}
