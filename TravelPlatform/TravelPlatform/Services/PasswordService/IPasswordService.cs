namespace TravelPlatform.Services.PasswordService
{
    public interface IPasswordService
    {
        Task<string> HashPassword(string password);
        Task<bool> VerifyPassword(string password, string hashedPassword);
    }
}
