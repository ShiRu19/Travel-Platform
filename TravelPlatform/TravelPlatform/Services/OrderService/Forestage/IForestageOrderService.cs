using Microsoft.AspNetCore.Mvc;
using TravelPlatform.Models;
using TravelPlatform.Models.Order;

namespace TravelPlatform.Services.OrderService.Forestage
{
    public interface IForestageOrderService
    {
        Task<ResponseDto> GenerateOrderAsync(OrderAddModel orderAddModel);
        Task<ResponseDto> GetOrderDetailAsync(int orderId);
        Task<ResponseDto> GetUserOrderPageCountAsync(int userId);
        Task<ResponseDto> GetUserOrderListAsync(int userId, int paging);
        Task<ResponseDto> UpdateOrderPayStatusAsync(OrderPaymentUpdateModel orderPaymentUpdateModel);
    }
}
