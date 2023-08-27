using TravelPlatform.Handler;
using TravelPlatform.Models.Domain;

namespace TravelPlatform.Services
{
    public interface ITokenService
    {
        Task<string> GenerateJwtToken(User user);
    }

    public class TokenService : ITokenService
    {
        private readonly ITokenHandler _tokenHandler;
        public TokenService(ITokenHandler tokenHandler) 
        {
            _tokenHandler = tokenHandler;
        }

        public async Task<string> GenerateJwtToken(User user)
        {
            string token = await _tokenHandler.GenerateJwtToken(user);
            return token;
        }
    }
}
