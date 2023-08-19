namespace TravelPlatform.Models.BackstageTravel
{
    public class TravelAddModel
    {
        public TravelInfoModel TravelInfo { get; set; } = null!;
        public ICollection<string> TravelAttraction { get; set; } = new List<string>();
        public ICollection<TravelSessionModel> TravelSession { get; set; } = new List<TravelSessionModel>();
    }
}
