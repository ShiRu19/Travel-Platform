using TravelPlatform.Models;
using TravelPlatform.Models.User;

namespace TravelPlatform.Services.UserService
{
    public interface IUserService
    {
        Task<ResponseDto> ProfileAsync();
        Task<ResponseDto> GetUserListAsync();
        Task<ResponseDto> SignUpAsync(SignUpModel user);
        Task<ResponseDto> SignInAsync(SignInModel user);
        Task<ResponseDto> CreateNewFBUserAsync(FBProfile profile);
    }
}
