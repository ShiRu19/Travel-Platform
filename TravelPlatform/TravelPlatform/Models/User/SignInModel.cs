namespace TravelPlatform.Models.User
{
    public class SignInModel
    {
        public string Povider { get; set; } = null!;
        public string? Email { get; set; }
        public string? Password { get; set; }
        public string? Access_tokent { get; set; }
    }
}
