namespace TravelPlatform.Models.Order
{
    public class OrderPaymentUpdateModel
    {
        public long OrderId { get; set; }
        public long UserId { get; set; }
        public string AccountDigits { get; set; } = null!;
    }
}
