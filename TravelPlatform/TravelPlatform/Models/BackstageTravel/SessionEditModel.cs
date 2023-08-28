namespace TravelPlatform.Models.BackstageTravel
{
    public class SessionEditModel
    {
        public long SessionId { get; set; }
        public TravelSessionModel TravelSession { get; set; } = null!;
    }
}
