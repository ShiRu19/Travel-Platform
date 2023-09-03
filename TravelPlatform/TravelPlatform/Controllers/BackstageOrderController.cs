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
                var user = _db.Users.Where(u => u.Id == order.UserId).Single();
                var qty = _db.OrderLists.Where(o => o.OrderId == order.Id).Count();

                OrderListDto orderDto = new OrderListDto()
                {
                    OrderId = order.Id,
                    ProductNumber = travelSession.ProductNumber,
                    Qty = qty,
                    Total = travelSession.Price * qty,
                    UserName = user.Name,
                    UserEmail = user.Email,
                    OrderDate = order.OrderDate
                };

                if(checkStatus != 0)
                {
                    orderDto.CheckDate = order.CheckDate;
                }

                OrderList.Add(orderDto);
            }

            return OrderList;
        }

        [MapToApiVersion("1.0")]
        [HttpPost("ChangeCheckedStatus")]
        public IActionResult ChangeCheckedStatus(CheckModel checkModel)
        {
            using (var transaction = _db.Database.BeginTransaction())
            {
                var order = _db.Orders.Where(o => o.Id == checkModel.OrderId).SingleOrDefault();

                if(order == null)
                {
                    return BadRequest(new
                    {
                        error = "Order id is not found.",
                        message = "Please confirm whether the order id exists."
                    });
                }

                order.CheckStatus = checkModel.Status == "checked" ? 1 : 2;
                order.CheckDate = DateTime.Now;

                try
                {
                    _db.SaveChanges();
                    transaction.Commit();
                    return Ok();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();

                    Console.WriteLine("Transaction rolled back due to an error: " + ex.Message);
                    return StatusCode(500, ex.Message);
                }
            }
        }
    }
}
