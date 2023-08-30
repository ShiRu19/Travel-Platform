namespace TravelPlatform.Models.Order
{
    public class OrderListDto
    {
        public long OrderId { get; set; }
        public string ProductNumber { get; set; } = null!;
        public int Qty { get; set; }
        public int Total { get; set; }
        public string UserName { get; set; } = null!;
        public string UserEmail { get; set; } = null!;
        public DateTime OrderDate { get; set; }
        public DateTime? CheckDate { get; set; }
    }
}
