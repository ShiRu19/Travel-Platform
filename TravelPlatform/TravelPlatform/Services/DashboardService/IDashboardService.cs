using TravelPlatform.Models;

namespace TravelPlatform.Services.DashboardService
{
    public interface IDashboardService
    {
        Task<ResponseDto> GetDomesticGroupStatusAsync(DateTime start, DateTime end);
        Task<ResponseDto> GetDomesticSalesVolumeAsync(DateTime start, DateTime end);
        Task<ResponseDto> GetFollowTopFiveAsync();
        Task<ResponseDto> GetSalesAsync(string nation, DateTime start, DateTime end);
        Task<ResponseDto> GetSalesVolumeAsync(string nation, DateTime start, DateTime end);
    }
}
