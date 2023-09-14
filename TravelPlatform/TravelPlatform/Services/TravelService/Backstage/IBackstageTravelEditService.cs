using TravelPlatform.Models.BackstageTravel;
using TravelPlatform.Models;

namespace TravelPlatform.Services.TravelService.Backstage
{
    public interface IBackstageTravelEditService
    {
        Task<ResponseDto> EditSessionAsync(SessionEditModel sessionEditModel);
        Task<ResponseDto> EditTravelAsync(TravelEditModel travelEditModel);
    }
}
