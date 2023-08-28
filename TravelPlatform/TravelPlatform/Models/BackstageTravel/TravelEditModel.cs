namespace TravelPlatform.Models.BackstageTravel
{
    public class TravelEditModel
    {
        public long Id { get; set; }
        public TravelInfoModel TravelInfo { get; set; } = null!;
        public ICollection<string> TravelAttraction { get; set; } = new List<string>();
    }
}
