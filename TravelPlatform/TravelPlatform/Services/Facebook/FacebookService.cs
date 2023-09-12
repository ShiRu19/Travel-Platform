using TravelPlatform.Handler.Facebook;
using TravelPlatform.Models.User;

namespace TravelPlatform.Services.Facebook
{
    public class FacebookService : IFacebookService
    {
        public readonly IFacebookHandler _facebookHandler;

        public FacebookService(IFacebookHandler facebookHandler)
        {
            _facebookHandler = facebookHandler;
        }

        public async Task<FBProfile> GetProfileAsync(string accessToken)
        {
            var result = await _facebookHandler.GetAsync<dynamic>(
                accessToken, "me", "fields=id,name,email");

            if (result == null)
            {
                return new FBProfile();
            }

            var profile = new FBProfile
            {
                Id = result.id,
                Email = result.email,
                Name = result.name
            };

            return profile;
        }

        public async Task PostOnWallAsync(string accessToken, string message)
            => await _facebookHandler.PostAsync(accessToken, "me/feed", new { message });
    }
}
