using Microsoft.AspNetCore.Mvc;
using TravelPlatform.Models;
using TravelPlatform.Models.BackstageTravel;

namespace TravelPlatform.Services.TravelService.Backstage
{
    public interface IBackstageTravelInfoService
    {
        Task<ResponseDto> GetSessionInfoAsync(long id);
        Task<ResponseDto> GetTravelInfoAsync(long id);
    }
}
