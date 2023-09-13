using TravelPlatform.Models;
using TravelPlatform.Models.BackstageOrder;

namespace TravelPlatform.Services.OrderService.Backstage
{
    public interface IBackstageOrderService
    {
        Task<ResponseDto> CancelOrderAsync(CancelOrderModel cancelOrderModel);
        Task<ResponseDto> CheckOrderAsync(CheckOrderModel checkOrderModel);
        Task<ResponseDto> GenerateOrderListAsync(int checkStatus, int page, string? productNumber);
        Task<ResponseDto> GetOrderPageCountAsync(string? productNumber);
    }
}
