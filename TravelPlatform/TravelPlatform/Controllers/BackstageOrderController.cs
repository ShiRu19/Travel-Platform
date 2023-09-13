using Azure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using TravelPlatform.Handler.Response;
using TravelPlatform.Models;
using TravelPlatform.Models.BackstageOrder;
using TravelPlatform.Models.Domain;
using TravelPlatform.Models.Order;
using TravelPlatform.Models.Record;
using TravelPlatform.Services.OrderService.Backstage;

namespace TravelPlatform.Controllers
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    [Authorize(Roles = "Admin")]
    public class BackstageOrderController : ControllerBase
    {
        private readonly TravelContext _db;
        private readonly IBackstageOrderService _orderService;
        private readonly IResponseHandler _responseHandler;

        public BackstageOrderController(TravelContext db, IBackstageOrderService orderService, IResponseHandler responseHandler)
        {
            _db = db;
            _orderService = orderService;
            _responseHandler = responseHandler;
        }

        /// <summary>
        /// 取得各訂單狀態頁數
        /// </summary>
        /// <param name="productNumber"></param>
        /// <returns></returns>
        [MapToApiVersion("1.0")]
        [HttpGet("GetOrderPageCount")]
        public async Task<IActionResult> GetOrderPageCount(string? productNumber)
        {
            if (productNumber == null)
            {
                productNumber = "all";
            }

            var response = await _orderService.GetOrderPageCountAsync(productNumber);
            return _responseHandler.ReturnResponse(response);
        }

        /// <summary>
        /// 取得訂單列表
        /// </summary>
        /// <param name="page"></param>
        /// <param name="productNumber"></param>
        /// <param name="orderType">0: unchecked; 1: checked; 2: canceled</param>
        /// <returns></returns>
        [MapToApiVersion("1.0")]
        [HttpGet("GetOrderList")]
        public async Task<IActionResult> GetOrderList(int page, string? productNumber, int orderType)
        {
            if (productNumber == null)
            {
                productNumber = "all";
            }

            var response = await _orderService.GenerateOrderListAsync(orderType, page, productNumber);
            return _responseHandler.ReturnResponse(response);
        }

        /// <summary>
        /// 取消訂單
        /// </summary>
        /// <param name="cancelOrderModel"></param>
        /// <returns></returns>
        [MapToApiVersion("1.0")]
        [HttpPost("CancelOrder")]
        public async Task<IActionResult> CancelOrder(CancelOrderModel cancelOrderModel)
        {
            var response = await _orderService.CancelOrderAsync(cancelOrderModel);
            return _responseHandler.ReturnResponse(response);
        }

        /// <summary>
        /// 確認訂單
        /// </summary>
        /// <param name="checkOrderModel"></param>
        /// <returns></returns>
        [MapToApiVersion("1.0")]
        [HttpPost("CheckOrder")]
        public async Task<IActionResult> CheckOrder(CheckOrderModel checkOrderModel)
        {
            var response = await _orderService.CheckOrderAsync(checkOrderModel);
            return _responseHandler.ReturnResponse(response);
        }
    }
}
