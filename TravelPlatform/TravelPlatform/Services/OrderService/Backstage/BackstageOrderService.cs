using Amazon.S3.Model;
using Microsoft.EntityFrameworkCore;
using System.Data;
using TravelPlatform.Hubs;
using TravelPlatform.Models;
using TravelPlatform.Models.BackstageOrder;
using TravelPlatform.Models.Domain;
using TravelPlatform.Models.Order;

namespace TravelPlatform.Services.OrderService.Backstage
{
    public class BackstageOrderService : IBackstageOrderService
    {
        private readonly TravelContext _db;
        private readonly IConfiguration _configuration;

        public BackstageOrderService(TravelContext db, IConfiguration configuration)
        {
            _db = db;
            _configuration = configuration;
        }

        /// <summary>
        /// 取消訂單
        /// </summary>
        /// <param name="cancelOrderModel"></param>
        /// <returns></returns>
        public async Task<ResponseDto> CancelOrderAsync(CancelOrderModel cancelOrderModel)
        {
            ResponseDto response200 = new ResponseDto() { StatusCode = 200, Message = "", Data = new { } };
            ResponseDto response500 = new ResponseDto() { StatusCode = 500, Message = "", Error = ""};
            ResponseDto response400 = new ResponseDto() { StatusCode = 400, Message = "", Error = "" };

            using (var transaction = _db.Database.BeginTransaction(IsolationLevel.ReadCommitted))
            {
                var orderId = cancelOrderModel.OrderId;
                var query = $"SELECT * FROM [Order] WITH (UPDLOCK, HOLDLOCK) WHERE [id] = {orderId}";

                Order order = new Order();

                try
                {
                    var order_query = _db.Orders.FromSqlRaw(query).FirstOrDefault();

                    if (order_query == null)
                    {
                        transaction.Rollback();
                        Console.WriteLine("Transaction rolled back due to an error: " + "This order does not exist.");

                        response400.StatusCode = 404;
                        response400.Error = _configuration["ErrorMessage:NOT_FOUND"];
                        response400.Message = "The order does not exist";

                        return response400;
                    }
                    else
                    {
                        order = order_query;
                    }
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    Console.WriteLine("Transaction rolled back due to an error: " + ex.Message);

                    response500.Error = _configuration["ErrorMessage:DB_OP_EX"];
                    response500.Message = ex.Message;
                    return response500;
                }

                order.CheckStatus = 2; // Cancel
                order.CheckDate = DateTime.UtcNow;

                try
                {
                    _db.SaveChanges();
                    transaction.Commit();
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
        /// 確認訂單
        /// </summary>
        /// <param name="checkOrderModel"></param>
        /// <returns></returns>
        public async Task<ResponseDto> CheckOrderAsync(CheckOrderModel checkOrderModel)
        {
            ResponseDto response200 = new ResponseDto() { StatusCode = 200, Message = "", Data = new { } };
            ResponseDto response500 = new ResponseDto() { StatusCode = 500, Message = "", Error = "" };
            ResponseDto response400 = new ResponseDto() { StatusCode = 400, Message = "", Error = "" };

            using (var transaction = _db.Database.BeginTransaction(IsolationLevel.ReadCommitted))
            {
                /* =======================
                 *   Update check status
                 * =======================
                 */
                var orderId = checkOrderModel.OrderId;
                var query = $"SELECT * FROM [Order] WITH (UPDLOCK, HOLDLOCK) WHERE [id] = {orderId}";

                Order order = new Order();

                try
                {
                    var order_query = _db.Orders.FromSqlRaw(query).FirstOrDefault();

                    if (order_query == null)
                    {
                        transaction.Rollback();
                        Console.WriteLine("Transaction rolled back due to an error: " + "This order does not exist.");

                        response400.StatusCode = 404;
                        response400.Error = _configuration["ErrorMessage:NOT_FOUND"];
                        response400.Message = "The order does not exist";

                        return response400;
                    }
                    else
                    {
                        order = order_query;
                    }
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    Console.WriteLine("Transaction rolled back due to an error: " + ex.Message);

                    response500.Error = _configuration["ErrorMessage:DB_OP_EX"];
                    response500.Message = ex.Message;
                    return response500;
                }

                order.CheckStatus = 1; // checked
                order.CheckDate = DateTime.UtcNow;

                /* ==================================
                 *   Update session remaining seats
                 * ==================================
                 */
                var sessionId = checkOrderModel.SessionId;
                var query_session = $"SELECT * FROM [TravelSession] WITH (UPDLOCK, HOLDLOCK) WHERE [id] = {sessionId}";
                TravelSession session = new TravelSession();

                try
                {
                    var session_query = _db.TravelSessions.FromSqlRaw(query_session).FirstOrDefault();

                    if (session_query == null)
                    {
                        transaction.Rollback();
                        Console.WriteLine("Transaction rolled back due to an error: " + "This session does not exist.");

                        response400.StatusCode = 404;
                        response400.Error = _configuration["ErrorMessage:NOT_FOUND"];
                        response400.Message = "The session does not exist";

                        return response400;
                    }

                    session = session_query;
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    Console.WriteLine("Transaction rolled back due to an error: " + ex.Message);

                    response500.Error = _configuration["ErrorMessage:DB_OP_EX"];
                    response500.Message = ex.Message;
                    return response500;
                }

                if (session.RemainingSeats < checkOrderModel.OrderSeats)
                {
                    transaction.Rollback();
                    Console.WriteLine("Transaction rolled back due to an error: " + "This session does not exist.");

                    response400.StatusCode = 400;
                    response400.Error = _configuration["ErrorMessage:BAD_REQUEST"];
                    response400.Message = "Not enough seats";

                    return response400;
                }

                session.RemainingSeats -= checkOrderModel.OrderSeats;

                /* ================
                 *   Save changes  
                 * ================
                 */
                try
                {
                    _db.SaveChanges();
                    transaction.Commit();
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
        /// 產生訂單列表
        /// </summary>
        /// <param name="checkStatus"></param>
        /// <param name="page"></param>
        /// <param name="productNumber"></param>
        /// <returns></returns>
        public async Task<ResponseDto> GenerateOrderListAsync(int checkStatus, int page, string productNumber)
        {
            ResponseDto response200 = new ResponseDto() { StatusCode = 200, Message = "", Data = new List<OrderListDto>() };
            ResponseDto response500 = new ResponseDto() { StatusCode = 500, Message = "", Error = "" };

            /* ========================
             *   Get order (6 items)
             * ========================
             */
            var orders = new List<Order>();
            page = Math.Max(1, page);

            try
            {
                if (productNumber == "all")
                {
                    orders = _db.Orders.Where(o => o.CheckStatus == checkStatus)
                                                .OrderBy(o => o.OrderDate)
                                                .Skip(5 * (page - 1))
                                                .Take(5)
                                                .ToList();
                }
                else
                {
                    var travelSession = _db.TravelSessions.Where(t => t.ProductNumber.ToLower() == productNumber.ToLower()).SingleOrDefault();
                    if (travelSession == null)
                    {
                        return response200;
                    }

                    orders = _db.Orders.Where(o => o.CheckStatus == checkStatus && o.TravelSessionId == travelSession.Id)
                                                .OrderBy(o => o.OrderDate)
                                                .Skip(5 * (page - 1))
                                                .Take(5)
                                                .ToList();
                }
            }
            catch (Exception ex)
            {
                response500.Error = _configuration["ErrorMessage:DB_OP_EX"];
                response500.Message = ex.Message;
                return response500;
            }

            /* =================================
             *   Genarate order information
             * =================================
             */
            List<OrderListDto> OrderList = new List<OrderListDto>();
            try
            {
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

                    if (order.PayStatus != 0)
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

                response200.Data = OrderList;
                return response200;
            }
            catch (Exception ex)
            {
                response500.Error = _configuration["ErrorMessage:DB_OP_EX"];
                response500.Message = ex.Message;
                return response500;
            }
        }

        /// <summary>
        /// 取得各訂單狀態頁數
        /// </summary>
        /// <param name="productNumber"></param>
        /// <returns></returns>
        public async Task<ResponseDto> GetOrderPageCountAsync(string productNumber)
        {
            ResponseDto response200 = new ResponseDto() { StatusCode = 200, Message = "", Data = new { } };
            ResponseDto response500 = new ResponseDto() { StatusCode = 500, Message = "", Error = "" };

            int count_unchecked = 0, count_checked = 0, count_canceled = 0;

            try
            {
                if(productNumber != "all")
                {
                    var travelSession = _db.TravelSessions.Where(t => t.ProductNumber.ToLower() == productNumber.ToLower()).SingleOrDefault();
                    if (travelSession != null)
                    {
                        count_unchecked = _db.Orders.Where(o => o.CheckStatus == 0 && o.TravelSessionId == travelSession.Id).Count();
                        count_checked = _db.Orders.Where(o => o.CheckStatus == 1 && o.TravelSessionId == travelSession.Id).Count();
                        count_canceled = _db.Orders.Where(o => o.CheckStatus == 2 && o.TravelSessionId == travelSession.Id).Count();
                    }
                }
                else
                {
                    count_unchecked = _db.Orders.Where(o => o.CheckStatus == 0).Count();
                    count_checked = _db.Orders.Where(o => o.CheckStatus == 1).Count();
                    count_canceled = _db.Orders.Where(o => o.CheckStatus == 2).Count();
                }
            }
            catch (Exception ex) 
            {
                response500.Error = _configuration["ErrorMessage:DB_OP_EX"];
                response500.Message = ex.Message;
                return response500;
            }

            var pagings_unchecked = Convert.ToInt32(Math.Ceiling(count_unchecked / 5.0));
            var pagings_checked = Convert.ToInt32(Math.Ceiling(count_checked / 5.0));
            var pagings_canceled = Convert.ToInt32(Math.Ceiling(count_canceled / 5.0));

            response200.Data = new
            {
                pagings_unchecked,
                pagings_checked,
                pagings_canceled
            };
            return response200;
        }
    }
}
