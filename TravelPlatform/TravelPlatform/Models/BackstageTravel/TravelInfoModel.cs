namespace TravelPlatform.Models.BackstageTravel
{
    public class TravelInfoModel
    {
        public string Title { get; set; } = null!;
        public DateTime DateRangeStart { get; set; }
        public DateTime DateRangeEnd { get; set; }
        public int Days { get; set; }
        public string DepartureLocation { get; set; } = null!;
        public IFormFile? PdfFile { get; set; }
        public IFormFile? MainImageFile { get; set; }
    }
}
