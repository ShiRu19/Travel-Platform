using TravelPlatform.Models;

namespace TravelPlatform.Services.TravelService.Backstage
{
    public interface IBackstageTravelListService
    {
        Task<ResponseDto> GetCloseTravelListAsync();
        Task<ResponseDto> GetCloseTravelSessionListAsync(long id);
        Task<ResponseDto> GetOpenTravelListAsync();
        Task<ResponseDto> GetOpenTravelSessionListAsync(long id);
    }
}
