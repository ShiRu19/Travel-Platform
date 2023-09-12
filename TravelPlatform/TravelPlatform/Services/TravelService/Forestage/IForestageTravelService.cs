using TravelPlatform.Models;

namespace TravelPlatform.Services.Travel.Forestage
{
    public interface IForestageTravelService
    {
        Task<ResponseDto> GetOpenTravelList();
        Task<ResponseDto> GetTravelDetail(long id);
        Task<ResponseDto> GetSessionDetail(string productNumber);
    }
}
