using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using TravelPlatform.Models.BackstageOrder;
using TravelPlatform.Models.Domain;
using TravelPlatform.Models.Order;

namespace TravelPlatform.Controllers
{
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
                var qty = _db.OrderLists.Where(o => o.OrderId == order.Id).Count();

                OrderListDto orderDto = new OrderListDto()
                {
                    OrderId = order.Id,
                    SessionId = travelSession.Id,
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

        [MapToApiVersion("1.0")]
        [HttpPost("CancelOrder")]
        public IActionResult CancelOrder(CancelOrderModel cancelOrderModel)
        {
            using (var transaction = _db.Database.BeginTransaction())
            {
                try
                {
                    var order = _db.Orders.Where(o => o.Id == cancelOrderModel.OrderId).SingleOrDefault();

                    if (order == null)
                    {
                        return BadRequest(new
                        {
                            error = "Order id is not found.",
                            message = "Please confirm whether the order id exists."
                        });
                    }

                    order.CheckStatus = 2; // Cancel
                    order.CheckDate = DateTime.UtcNow;

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

        [MapToApiVersion("1.0")]
        [HttpPost("CheckOrder")]
        public IActionResult CheckOrder(CheckOrderModel checkOrderModel)
        {
            using (var transaction = _db.Database.BeginTransaction())
            {
                try
                {
                    // Update check status
                    var order = _db.Orders.Where(o => o.Id == checkOrderModel.OrderId).SingleOrDefault();

                    if (order == null)
                    {
                        return BadRequest(new
                        {
                            error = "Order id is not found.",
                            message = "Please confirm whether the order id exists."
                        });
                    }

                    order.CheckStatus = 1; // checked
                    order.CheckDate = DateTime.UtcNow;

                    // Update session remaining seats
                    var session = _db.TravelSessions.Where(t => t.Id == checkOrderModel.SessionId).SingleOrDefault();

                    if (session == null)
                    {
                        return BadRequest(new
                        {
                            error = "Session id is not found.",
                            message = "Please comfirm the id is exists."
                        });
                    }

                    if (session.RemainingSeats < checkOrderModel.OrderSeats)
                    {
                        return BadRequest(new
                        {
                            error = "Not enough seats.",
                            message = "Please adjust the quantity."
                        });
                    }

                    session.RemainingSeats -= checkOrderModel.OrderSeats;

                    // Save changes
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
