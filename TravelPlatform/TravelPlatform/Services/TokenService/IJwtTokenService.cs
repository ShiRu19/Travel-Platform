using TravelPlatform.Models.Domain;

namespace TravelPlatform.Services.Token
{
    public interface IJwtTokenService
    {
        Task<string> GenerateJwtToken(User user);
    }
}
