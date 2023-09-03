namespace TravelPlatform.Models.Order
{
    public class OrderAddModel
    {
        public string Nation { get; set; } = null!;
        public long SessionId { get; set; }
        public long Total { get; set; }
        public long UserId { get; set; }
        public string UserName { get; set; } = null!;
        public string UserEmail { get; set; } = null!;
        public string UserPhone { get; set; } = null!;
        public ICollection<OrderTravelerAddModel> OrderTravelers { get; set; } = new List<OrderTravelerAddModel>();
    }
}
