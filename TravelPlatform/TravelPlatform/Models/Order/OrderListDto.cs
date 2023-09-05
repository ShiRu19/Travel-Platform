namespace TravelPlatform.Models.Order
{
    public class OrderListDto
    {
        // Order
        public long OrderId { get; set; }
        public string ProductNumber { get; set; } = null!;
        public int Qty { get; set; }
        public int Total { get; set; }
        public DateTime OrderDate { get; set; }

        // User Information
        public string UserName { get; set; } = null!;
        public string UserEmail { get; set; } = null!;
        public string UserPhone { get; set; } = null!;
        
        // Payment
        public int PayStatus { get; set; }
        public string? AccountDigits { get; set; }
        public DateTime? PayDate { get; set; }

        // Check
        public int CheckStatus { get; set; }
        public DateTime? CheckDate { get; set; }
    }
}
