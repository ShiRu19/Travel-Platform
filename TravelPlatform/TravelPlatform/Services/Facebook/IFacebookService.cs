using TravelPlatform.Models.User;

namespace TravelPlatform.Services.Facebook
{
    public interface IFacebookService
    {
        Task<FBProfile> GetProfileAsync(string accessToken);
        Task PostOnWallAsync(string accessToken, string message);
    }
}
