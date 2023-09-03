namespace TravelPlatform.Models.Order
{
    public class OrderTravelerAddModel
    {
        public string Name { get; set; } = null!;
        public string Sex { get; set; } = null!;
        public DateTime Birthday { get; set; }
        public string PhoneNumber { get; set; } = null!;
        public int Price { get; set; }

        public string? LastName { get; set; }

        public string? FirstName { get; set; }

        public string? IdentityCode { get; set; }

        public string? PassportNumber { get; set; }

        public string? SpecialNeed { get; set; }
    }
}
