using TravelPlatform.Models.BackstageTravel;
using TravelPlatform.Models;

namespace TravelPlatform.Services.TravelService.Backstage
{
    public interface IBackstageTravelAddService
    {
        Task<ResponseDto> AddSessionAsync(SessionAddModel sessionAddModel);
        Task<ResponseDto> AddTravelAsync(TravelAddModel travelAddModel);
    }
}
