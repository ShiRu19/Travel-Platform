using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TravelPlatform.Models.Domain;
using TravelPlatform.Models.Order;

namespace TravelPlatform.Controllers
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    public class OrderController : ControllerBase
    {
        private readonly TravelContext _db;
        public OrderController(TravelContext db)
        {
            _db = db;
        }

        [MapToApiVersion("1.0")]
        [HttpPost("GenerateOrder")]
        public IActionResult GenerateOrder([FromForm] OrderAddModel orderAddModel)
        {
            using (var transaction = _db.Database.BeginTransaction())
            {
                try
                {
                    Order order = new Order
                    {
                        Id = _db.Orders.Count() == 0 ? 1 : _db.Orders.Max(o => o.Id) + 1,
                        OrderDate = DateTime.UtcNow,
                        Nation = orderAddModel.Nation,
                        TravelSessionId = orderAddModel.SessionId,
                        Total = orderAddModel.Total,
                        UserId = orderAddModel.UserId,
                        UserName = orderAddModel.UserName,
                        UserEmail = orderAddModel.UserEmail,
                        UserPhoneNumber = orderAddModel.UserPhone,
                        PayStatus = 0,
                        CheckStatus = 0
                    };

                    long orderListId = _db.OrderLists.Count() == 0 ? 1 : _db.OrderLists.Max(o => o.Id) + 1;

                    foreach (var traveler in orderAddModel.OrderTravelers)
                    {
                        OrderList orderList = new OrderList
                        {
                            Id = orderListId++,
                            OrderId = order.Id,
                            Price = traveler.Price,
                            Name = traveler.Name,
                            Sex = traveler.Sex,
                            Birthday = traveler.Birthday,
                            PhoneNumber = traveler.PhoneNumber
                        };

                        if(traveler.LastName != null)
                        {
                            orderList.LastName = traveler.LastName;
                        }
                        if (traveler.FirstName != null)
                        {
                            orderList.FirstName = traveler.FirstName;
                        }
                        if (traveler.IdentityCode != null)
                        {
                            orderList.IdentityCode = traveler.IdentityCode;
                        }
                        if (traveler.PassportNumber != null)
                        {
                            orderList.PassportNumber = traveler.PassportNumber;
                        }
                        if (traveler.SpecialNeed != null)
                        {
                            orderList.SpecialNeed = traveler.SpecialNeed;
                        }

                        _db.OrderLists.Add(orderList);
                    }

                    _db.Orders.Add(order);
                    _db.SaveChanges();
                    transaction.Commit();
                    Console.WriteLine("Transaction committed successfully.");
                    return Ok(new
                    {
                        orderId = order.Id
                    });
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
        [HttpPost("UpdateOrderPayStatus")]
        public IActionResult UpdateOrderPayStatus([FromForm] OrderPaymentUpdateModel orderPaymentUpdateModel)
        {
            using (var transaction = _db.Database.BeginTransaction())
            {
                try
                {
                    var order = _db.Orders.Where(o => o.Id == orderPaymentUpdateModel.OrderId && o.UserId == orderPaymentUpdateModel.UserId).FirstOrDefault();

                    if(order == null)
                    {
                        return BadRequest(new
                        {
                            error = "This order is not found.",
                            message = "Please confirm whether the user has this order."
                        });
                    }

                    order.PayStatus = 1;
                    order.AccountFiveDigits = orderPaymentUpdateModel.AccountDigits;
                    order.PayDate = DateTime.UtcNow;

                    _db.SaveChanges();
                    transaction.Commit();
                    return Ok(new
                    {
                        id = order.Id
                    });
                }
                catch(Exception ex)
                {
                    transaction.Rollback();

                    Console.WriteLine("Transaction rolled back due to an error: " + ex.Message);
                    return StatusCode(500, ex.Message);
                }
            }
        }

        [MapToApiVersion("1.0")]
        [HttpGet("GetOrderDetail")]
        public IActionResult GetOrderDetail(int orderId)
        {
            var orderList = _db.OrderLists.Where(o => o.OrderId == orderId)
                                        .Select(o => new
                                        {
                                            name = o.Name,
                                            sex = o.Sex == "man" ? "男" : "女",
                                            birthday = o.Birthday,
                                            phoneNumber = o.PhoneNumber
                                        })
                                        .ToList();
            return Ok(new
            {
                orderId = orderId,
                qty = orderList.Count(),
                travelers = orderList
            });
        }

        [MapToApiVersion("1.0")]
        [HttpGet("GetUserOrderPageCount")]
        [Authorize]
        public IActionResult GetUserOrderPageCount(int userId)
        {
            var count = _db.Orders.Where(o => o.UserId == userId).Count();
            var pagings = Convert.ToInt32(Math.Ceiling(count / 6.0));
            return Ok(pagings);
        }

        [MapToApiVersion("1.0")]
        [HttpGet("GetUserOrderList")]
        [Authorize]
        public IActionResult GetUserOrderList(int userId, int paging)
        {
            if(paging == 0)
            {
                paging = 1;
            }
            var orders = _db.Orders.Where(o => o.UserId == userId)
                                .OrderByDescending(o => o.OrderDate)
                                .Skip((paging-1) * 6)
                                .Take(6)
                                .ToList();
            if(orders == null)
            {
                return NotFound();
            }

            List<UserOrderDto> userOrderListDto = new List<UserOrderDto>();

            foreach(var order in orders)
            {
                var qty = _db.OrderLists.Where(o => o.OrderId == order.Id).Count();

                var session = _db.TravelSessions.Where(t => t.Id == order.TravelSessionId).SingleOrDefault();
                if(session == null)
                {
                    return BadRequest(new
                    {
                        error = "The session id is not found.",
                        message = "Please confirm whether this order data is correct."
                    });
                }

                var travel = _db.Travels.Where(t => t.Id == session.TravelId).SingleOrDefault();
                if(travel == null)
                {
                    return BadRequest(new
                    {
                        error = "The travel id is not found.",
                        message = "Please confirm whether this order data is correct."
                    });
                }

                UserOrderDto userOrder = new UserOrderDto()
                {
                    OrderId = order.Id,
                    Title = travel.Title,
                    Price = session.Price,
                    Qty = qty,
                    OrderDate = order.OrderDate,
                    CheckStatus = order.CheckStatus
                };
                userOrderListDto.Add(userOrder);
            }

            return Ok(userOrderListDto);
        }
    }
}
