using Microsoft.AspNetCore.Mvc;
using TravelPlatform.Models;
using TravelPlatform.Models.Record;

namespace TravelPlatform.Services.Record
{
    public interface IFollowService
    {
        Task<ResponseDto> AddFollowAsync(FollowModel followModel);
        Task<ResponseDto> CancelFollowAsync(FollowModel followModel);
        Task<ResponseDto> CheckFollowAsync(FollowModel followModel);
        Task<ResponseDto> GetUserFollowListAsync(long UserId);
    }
}
