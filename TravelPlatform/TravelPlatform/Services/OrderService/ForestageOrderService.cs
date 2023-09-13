using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;
using TravelPlatform.Hubs;
using TravelPlatform.Models;
using TravelPlatform.Models.Domain;
using TravelPlatform.Models.Order;

namespace TravelPlatform.Services.OrderService
{
    public class ForestageOrderService : IForestageOrderService
    {
        private readonly TravelContext _db;
        private readonly IConfiguration _configuration;

        public ForestageOrderService(TravelContext db, IConfiguration configuration)
        {
            _db = db;
            _configuration = configuration;
        }

        /// <summary>
        /// 產生訂單
        /// </summary>
        /// <param name="orderAddModel"></param>
        /// <returns></returns>
        public async Task<ResponseDto> GenerateOrderAsync(OrderAddModel orderAddModel)
        {
            ResponseDto response200 = new ResponseDto() { StatusCode = 200, Message = "", Data = new { } };
            ResponseDto response500 = new ResponseDto() { StatusCode = 500, Message = "", Error = "" };

            using (var transaction = _db.Database.BeginTransaction(IsolationLevel.ReadCommitted))
            {
                Order order = new Order
                {
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
                long orderListId = 0;

                try
                {
                    order.Id = _db.Orders.Count() == 0 ? 1 : _db.Orders.Max(o => o.Id) + 1;
                    orderListId = _db.OrderLists.Count() == 0 ? 1 : _db.OrderLists.Max(o => o.Id) + 1;
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    Console.WriteLine("Transaction rolled back due to an error: " + ex.Message);

                    response500.Error = _configuration["ErrorMessage:DB_OP_EX"];
                    response500.Message = ex.Message;
                    return response500;
                }

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

                    if (traveler.LastName != null)
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

                try
                {
                    _db.SaveChanges();
                    transaction.Commit();
                    Console.WriteLine("Transaction committed successfully.");

                    response200.Data = order.Id;
                    return response200;
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    Console.WriteLine("Transaction rolled back due to an error: " + ex.Message);

                    response500.Error = _configuration["ErrorMessage:DB_SAVE_EX"];
                    response500.Message = ex.Message;
                    return response500;
                }
            }
        }

        /// <summary>
        /// 取得訂單詳情
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        public async Task<ResponseDto> GetOrderDetailAsync(int orderId)
        {
            ResponseDto response200 = new ResponseDto() { StatusCode = 200, Message = "", Data = new { } };
            ResponseDto response500 = new ResponseDto() { StatusCode = 500, Message = "", Error = "" };

            try
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
                
                response200.Data = new
                {
                    orderId,
                    qty = orderList.Count(),
                    travelers = orderList
                };

                return response200;
            }
            catch (Exception ex)
            {
                response500.Error = _configuration["ErrorMessage:DB_SAVE_EX"];
                response500.Message = ex.Message;
                return response500;
            }
        }

        /// <summary>
        /// 取得使用者訂單列表
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="paging"></param>
        /// <returns></returns>
        public async Task<ResponseDto> GetUserOrderListAsync(int userId, int paging)
        {
            ResponseDto response200 = new ResponseDto() { StatusCode = 200, Message = "", Data = new { } };
            ResponseDto response500 = new ResponseDto() { StatusCode = 500, Message = "", Error = "" };

            var orders = _db.Orders.Where(o => o.UserId == userId)
                                .OrderByDescending(o => o.OrderDate)
                                .Skip((paging - 1) * 6)
                                .Take(6)
                                .ToList();
            if (orders == null)
            {
                return response200;
            }

            try
            {
                List<UserOrderDto> userOrderListDto = new List<UserOrderDto>();
                foreach (var order in orders)
                {
                    var qty = _db.OrderLists.Where(o => o.OrderId == order.Id).Count();

                    var session = _db.TravelSessions.Where(t => t.Id == order.TravelSessionId).SingleOrDefault();
                    if (session == null)
                    {
                        response500.Error = _configuration["ErrorMessage:NOT_FOUND"];
                        response500.Message = "Session does not exist";
                        return response500;
                    }

                    var travel = _db.Travels.Where(t => t.Id == session.TravelId).SingleOrDefault();
                    if (travel == null)
                    {
                        response500.Error = _configuration["ErrorMessage:NOT_FOUND"];
                        response500.Message = "Travel does not exist";
                        return response500;
                    }

                    UserOrderDto userOrder = new UserOrderDto()
                    {
                        OrderId = order.Id,
                        Title = travel.Title,
                        ProductNumber = session.ProductNumber,
                        Price = session.Price,
                        Qty = qty,
                        OrderDate = order.OrderDate,
                        PayStatus = order.PayStatus,
                        CheckStatus = order.CheckStatus
                    };
                    userOrderListDto.Add(userOrder);
                }

                response200.Data = userOrderListDto;

                return response200;
            }
            catch (Exception ex)
            {
                response500.Error = _configuration["ErrorMessage:DB_SAVE_EX"];
                response500.Message = ex.Message;
                return response500;
            }
        }

        /// <summary>
        /// 取得使用者訂單頁數
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<ResponseDto> GetUserOrderPageCountAsync(int userId)
        {
            ResponseDto response200 = new ResponseDto() { StatusCode = 200, Message = "", Data = new { } };
            ResponseDto response500 = new ResponseDto() { StatusCode = 500, Message = "", Error = "" };
            try
            {
                var count = _db.Orders.Where(o => o.UserId == userId).Count();
                var pagings = Convert.ToInt32(Math.Ceiling(count / 6.0));

                response200.Data = pagings;
                return response200;
            }
            catch(Exception ex)
            {
                response500.Error = _configuration["ErrorMessage:DB_OP_EX"];
                response500.Message = ex.Message;
                return response500;
            }
        }

        /// <summary>
        /// 更新付款狀態
        /// </summary>
        /// <param name="orderPaymentUpdateModel"></param>
        /// <returns></returns>
        public async Task<ResponseDto> UpdateOrderPayStatusAsync(OrderPaymentUpdateModel orderPaymentUpdateModel)
        {
            ResponseDto response200 = new ResponseDto() { StatusCode = 200, Message = "", Data = new { } };
            ResponseDto response500 = new ResponseDto() { StatusCode = 500, Message = "", Error = "" };
            ResponseDto response400 = new ResponseDto();

            using (var transaction = _db.Database.BeginTransaction())
            {
                var orderId = orderPaymentUpdateModel.OrderId;
                var userId = orderPaymentUpdateModel.UserId;

                var query = $"SELECT * FROM [Order] WITH (UPDLOCK, HOLDLOCK) WHERE [id] = {orderId} AND [user_id] = {userId}";

                Order order = new Order();

                try
                {
                    var order_query = _db.Orders.FromSqlRaw(query).FirstOrDefault();
                    //var order = _db.Orders.Where(o => o.Id == orderId && o.UserId == userId).FirstOrDefault();
                    
                    if (order_query == null)
                    {
                        transaction.Rollback();
                        Console.WriteLine("Transaction rolled back due to an error: " + "This order does not exist.");

                        response400.StatusCode = 400;
                        response400.Error = _configuration["ErrorMessage:BAD_REQUEST"];
                        response400.Message = "This order does not exist.";
                        return response400;
                    }

                    order = order_query;
                }
                catch (Exception ex)
                {
                    response500.Error = _configuration["ErrorMessage:DB_OP_EX"];
                    response500.Message = ex.Message;
                    return response500;
                }

                order.PayStatus = 1;
                order.AccountFiveDigits = orderPaymentUpdateModel.AccountDigits;
                order.PayDate = DateTime.UtcNow;

                try
                {
                    _db.SaveChanges();
                    transaction.Commit();
                    
                    response200.Data = order.Id;
                    return response200;
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    Console.WriteLine("Transaction rolled back due to an error: " + ex.Message);

                    response500.Error = _configuration["ErrorMessage:DB_SAVE_EX"];
                    response500.Message = ex.Message;
                    return response500;
                }
            }
        }
    }
}
