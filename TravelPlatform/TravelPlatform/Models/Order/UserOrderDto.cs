namespace TravelPlatform.Models.Order
{
    public class UserOrderDto
    {
        public long OrderId { get; set; }
        public string Title { get; set; } = null!;
        public string ProductNumber { get; set; } = null!;
        public int Price { get; set; }
        public int Qty { get; set; }
        public DateTime OrderDate { get; set; }
        public int PayStatus { get; set; }
        public int CheckStatus { get; set; }
    }
}
