namespace TravelPlatform.Models.User
{
    public class UserProfile
    {
        public long Id { get; set; }
        public string Provider { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string Email { get; set; } = null!;
    }
}
