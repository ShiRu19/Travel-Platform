namespace TravelPlatform.Models.BackstageTravel
{
    public class SessionEditModel
    {
        public long TravelId { get; set; }
        public string SessionNumber { get; set; } = null!;
        public TravelSessionModel TravelSession { get; set; } = null!;
    }
}
