using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TravelPlatform.Handler.Response;
using TravelPlatform.Hubs;
using TravelPlatform.Models.Domain;
using TravelPlatform.Models.Order;
using TravelPlatform.Models.Record;
using TravelPlatform.Services.OrderService;

namespace TravelPlatform.Controllers
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    public class OrderController : ControllerBase
    {
        private readonly TravelContext _db;
        private readonly IForestageOrderService _orderService;
        private readonly IResponseHandler _responseHandler;

        public OrderController(TravelContext db, IForestageOrderService orderService, IResponseHandler responseHandler)
        {
            _db = db;
            _orderService = orderService;
            _responseHandler = responseHandler;
        }

        /// <summary>
        /// 產生訂單
        /// </summary>
        /// <param name="orderAddModel"></param>
        /// <returns></returns>
        [MapToApiVersion("1.0")]
        [HttpPost("GenerateOrder")]
        public async Task<IActionResult> GenerateOrder([FromForm] OrderAddModel orderAddModel)
        {
            var response = await _orderService.GenerateOrderAsync(orderAddModel);
            return _responseHandler.ReturnResponse(response);
        }

        /// <summary>
        /// 更新付款狀態
        /// </summary>
        /// <param name="orderPaymentUpdateModel"></param>
        /// <returns></returns>
        [MapToApiVersion("1.0")]
        [HttpPost("UpdateOrderPayStatus")]
        public async Task<IActionResult> UpdateOrderPayStatus([FromForm] OrderPaymentUpdateModel orderPaymentUpdateModel)
        {
            var response = await _orderService.UpdateOrderPayStatusAsync(orderPaymentUpdateModel);
            return _responseHandler.ReturnResponse(response);
        }

        /// <summary>
        /// 取得訂單詳情
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        [MapToApiVersion("1.0")]
        [HttpGet("GetOrderDetail")]
        public async Task<IActionResult> GetOrderDetail(int orderId)
        {
            var response = await _orderService.GetOrderDetailAsync(orderId);
            return _responseHandler.ReturnResponse(response);
        }

        /// <summary>
        /// 取得使用者訂單總頁數
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [MapToApiVersion("1.0")]
        [HttpGet("GetUserOrderPageCount")]
        [Authorize]
        public async Task<IActionResult> GetUserOrderPageCount(int userId)
        {
            var response = await _orderService.GetUserOrderPageCountAsync(userId);
            return _responseHandler.ReturnResponse(response);
        }

        /// <summary>
        /// 取得使用者訂單列表
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="paging"></param>
        /// <returns></returns>
        [MapToApiVersion("1.0")]
        [HttpGet("GetUserOrderList")]
        [Authorize]
        public async Task<IActionResult> GetUserOrderList(int userId, int paging)
        {
            if(paging == 0)
            {
                paging = 1;
            }

            var response = await _orderService.GetUserOrderListAsync(userId, paging);
            return _responseHandler.ReturnResponse(response);
        }
    }
}
