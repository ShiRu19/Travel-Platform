namespace TravelPlatform.Models.BackstageTravel
{
    public class TravelSessionList
    {
        public long Id { get; set; }
        public string ProductNumber { get; set; } = null!;
        public string Title { get; set; } = null!;
        public string departureDate { get; set; } = null!;
        public int Days { get; set; }
        public int Price { get; set; }
        public int RemainingSeats { get; set; }
        public int Seats { get; set; }
        public int GroupStatus { get; set; }
    }
}
