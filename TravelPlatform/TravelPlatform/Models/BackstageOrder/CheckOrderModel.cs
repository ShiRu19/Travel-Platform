namespace TravelPlatform.Models.BackstageOrder
{
    public class CheckOrderModel
    {
        public long OrderId { get; set; }
        public long SessionId { get; set; }
        public int OrderSeats { get; set; }
    }
}
