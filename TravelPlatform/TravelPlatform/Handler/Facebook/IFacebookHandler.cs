namespace TravelPlatform.Handler.Facebook
{
    public interface IFacebookHandler
    {
        Task<T> GetAsync<T>(string accessToken, string endpoint, string args = null);
        Task PostAsync(string accessToken, string endpoint, object data, string args = null);
    }
}
