namespace TravelPlatform.Models.BackstageTravel
{
    public class SessionAddModel
    {
        public long TravelId { get; set; }
        public ICollection<TravelSessionModel> TravelSession { get; set; } = new List<TravelSessionModel>();
    }
}
