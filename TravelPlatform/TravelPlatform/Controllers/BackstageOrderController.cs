using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using TravelPlatform.Models.Domain;
using TravelPlatform.Models.Order;

namespace TravelPlatform.Controllers
{
    public class CheckModel
    {
        public int OrderId { get; set; }
        public string Status { get; set; } = null!;
    }

    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    [Authorize(Roles = "Admin")]
    public class BackstageOrderController : ControllerBase
    {
        private readonly TravelContext _db;
        public BackstageOrderController(TravelContext db)
        {
            _db = db;
        }

        [MapToApiVersion("1.0")]
        [HttpGet("GetOrderList")]
        public IActionResult GetOrderList()
        {
            return Ok(new
            {
                order_unchecked = GenerateOrderList(0),
                order_checked = GenerateOrderList(1),
                order_canceled = GenerateOrderList(2)
            });
        }

        private List<OrderListDto> GenerateOrderList(int checkStatus)
        {
            var orders = _db.Orders.Where(o => o.CheckStatus == checkStatus)
                                                .OrderBy(o => o.OrderDate)
                                                .ToList();

            List<OrderListDto> OrderList = new List<OrderListDto>();

            foreach (var order in orders)
            {
                var travelSession = _db.TravelSessions.Where(t => t.Id == order.TravelSessionId).Single();
                //var user = _db.Users.Where(u => u.Id == order.UserId).Single();
                var qty = _db.OrderLists.Where(o => o.OrderId == order.Id).Count();

                OrderListDto orderDto = new OrderListDto()
                {
                    OrderId = order.Id,
                    ProductNumber = travelSession.ProductNumber,
                    Qty = qty,
                    Total = travelSession.Price * qty,
                    OrderDate = order.OrderDate,
                    UserName = order.UserName,
                    UserEmail = order.UserEmail,
                    UserPhone = order.UserPhoneNumber,
                    PayStatus = order.PayStatus,
                    CheckStatus = order.CheckStatus
                };

                if(order.PayStatus != 0)
                {
                    orderDto.AccountDigits = order.AccountFiveDigits;
                    orderDto.PayDate = order.PayDate;
                }

                if (checkStatus != 0)
                {
                    orderDto.CheckDate = order.CheckDate;
                }

                OrderList.Add(orderDto);
            }

            return OrderList;
        }
    }
}
