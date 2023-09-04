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
