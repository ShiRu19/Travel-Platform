using TravelPlatform.Models.Domain;

namespace TravelPlatform.Handler.Token
{
    public interface IJwtTokenHandler
    {
        Task<string> GenerateJwtToken(User user);
    }
}
