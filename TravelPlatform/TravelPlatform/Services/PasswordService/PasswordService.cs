namespace TravelPlatform.Services.PasswordService
{
    public class PasswordService : IPasswordService
    {
        public async Task<string> HashPassword(string password)
        {
            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(password);
            return hashedPassword;
        }

        public async Task<bool> VerifyPassword(string password, string hashedPassword)
        {
            bool result = BCrypt.Net.BCrypt.Verify(password, hashedPassword);
            return result;
        }
    }
}
