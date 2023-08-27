namespace TravelPlatform.Models.User
{
    public class SignInModel
    {
        public string Provider { get; set; } = null!;
        public string? Email { get; set; }
        public string? Password { get; set; }
        public string? Access_token_fb { get; set; }
    }
}
