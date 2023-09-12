using TravelPlatform.Handler.Token;
using TravelPlatform.Models.Domain;

namespace TravelPlatform.Services.Token
{
    public class JwtTokenService : IJwtTokenService
    {
        private readonly IJwtTokenHandler _tokenHandler;
        public JwtTokenService(IJwtTokenHandler tokenHandler)
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
