namespace TravelPlatform.Models.BackstageTravel
{
    public class TravelSessionModel
    {
        public string ProductNumber { get; set; } = null!;
        public int Price { get; set; }
        public DateTime DepartureDate { get; set; }
        public int Applicants { get; set; }
        public int Seats { get; set; }
        public int GroupStatus { get; set; }
    }
}
