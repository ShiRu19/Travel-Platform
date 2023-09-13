using Microsoft.EntityFrameworkCore;
using System.Data;
using TravelPlatform.Models;
using TravelPlatform.Models.Domain;

namespace TravelPlatform.Services.Travel.Forestage
{
    public class ForestageTravelService : IForestageTravelService
    {
        private readonly TravelContext _db;
        private readonly IConfiguration _configuration;

        public ForestageTravelService(TravelContext db, IConfiguration configuration)
        {
            _db = db;
            _configuration = configuration;
        }

        /// <summary>
        /// 取得開放中的行程列表
        /// </summary>
        /// <returns></returns>
        public async Task<ResponseDto> GetOpenTravelList()
        {
            ResponseDto response200 = new ResponseDto { StatusCode = 200, Message = "", Data = new { } };
            ResponseDto response500 = new ResponseDto { StatusCode = 500, Message = "", Error = "" };

            try
            {
                var travels = _db.Travels.Where(t => t.DateRangeEnd >= DateTime.Now)
                    .Select(t => new
                    {
                        t.Id,
                        t.Title,
                        t.DateRangeStart,
                        t.DateRangeEnd,
                        t.MainImageUrl
                    }).ToList();

                response200.Data = travels;
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
        /// 取得指定行程的詳細資訊
        /// </summary>
        /// <param name="id">行程 id</param>
        /// <returns></returns>
        public async Task<ResponseDto> GetTravelDetail(long id)
        {
            ResponseDto response200 = new ResponseDto { StatusCode = 200, Message = "", Data = new { } };
            ResponseDto response500 = new ResponseDto { StatusCode = 500, Message = "", Error = "" };
            ResponseDto response400 = new ResponseDto();

            using (var transaction = _db.Database.BeginTransaction(IsolationLevel.ReadCommitted))
            {
                try
                {
                    var travel = _db.Travels.Where(t => t.Id == id)
                            .Select(t => new
                            {
                                t.Title,
                                t.DateRangeStart,
                                t.DateRangeEnd,
                                t.Nation,
                                t.PdfUrl,
                                t.MainImageUrl
                            });

                    if(travel.Count() == 0)
                    {
                        transaction.Rollback();
                        Console.WriteLine("Transaction rolled back due to an error: " + "Not Found Exception");

                        response400.StatusCode = StatusCodes.Status404NotFound;
                        response400.Error = _configuration["ErrorMessage:NOT_FOUND"];
                        response400.Message = "Travel id does not exist";
                        return response400;
                    }

                    var sessions  = await _db.TravelSessions.Where(t => t.TravelId == id && t.DepartureDate >= DateTime.Now)
                        .Select(t => new
                        {
                            t.ProductNumber,
                            t.DepartureDate,
                            t.RemainingSeats,
                            t.Seats,
                            GroupStatus = t.GroupStatus == 1 ? "已成團" : "尚未成團",
                            t.Price
                        })
                        .ToListAsync();

                    transaction.Commit();
                    Console.WriteLine("Transaction committed successfully.");

                    response200.Data = new
                    {
                        travelInfo = travel,
                        travelSessions = sessions
                    };

                    return response200;
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    Console.WriteLine("Transaction rolled back due to an error: " + ex.Message);

                    response500.Error = _configuration["ErrorMessage:DB_OP_EX"];
                    response500.Message = ex.Message;
                    return response500;
                }
            }
        }

        /// <summary>
        /// 取得指定場次的詳細資訊
        /// </summary>
        /// <param name="productNumber">場次編號</param>
        /// <returns></returns>
        public async Task<ResponseDto> GetSessionDetail(string productNumber)
        {
            ResponseDto response200 = new ResponseDto { StatusCode = 200, Message = "", Data = new { } };
            ResponseDto response500 = new ResponseDto { StatusCode = 500, Message = "", Error = "" };
            ResponseDto response400 = new ResponseDto();

            using (var transaction = _db.Database.BeginTransaction(IsolationLevel.ReadCommitted))
            {
                TravelSession session = new TravelSession();
                Models.Domain.Travel travel = new Models.Domain.Travel();

                /* ===============
                 *  Query session
                 * ===============
                 */
                try
                {
                    var sessionQuery = _db.TravelSessions.Where(t => t.ProductNumber == productNumber).FirstOrDefault();
                    if (sessionQuery == null)
                    {
                        transaction.Rollback();
                        Console.WriteLine("Transaction rolled back due to an error: " + "Not Found Exception");

                        response400.StatusCode = StatusCodes.Status404NotFound;
                        response400.Error = _configuration["ErrorMessage:NOT_FOUND"];
                        response400.Message = "Product number does not exist";
                        return response400;
                    }
                    else
                    {
                        session = sessionQuery;
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

                /* ==============
                 *  Query travel
                 * ==============
                 */
                try
                {
                    var travelQuery = _db.Travels.Where(t => t.Id == session.TravelId).FirstOrDefault();
                    if(travelQuery == null)
                    {
                        transaction.Rollback();
                        Console.WriteLine("Transaction rolled back due to an error: " + "Not Found Exception");

                        response400.StatusCode = StatusCodes.Status404NotFound;
                        response400.Error = _configuration["ErrorMessage:NOT_FOUND"];
                        response400.Message = "Travel does not exist";
                        return response400;
                    }
                    else
                    {
                        travel = travelQuery;
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

                transaction.Commit();
                Console.WriteLine("Transaction committed successfully.");

                /* ================
                 *  Departure date
                 * ================
                 */
                var startDate = session.DepartureDate;
                var endDate = session.DepartureDate.AddDays(travel.Days - 1);

                /* ==========
                 *  Response
                 * ==========
                 */
                response200.Data = new
                {
                    title = travel.Title,
                    departure_date_start = startDate,
                    departure_date_end = endDate,
                    days = travel.Days,
                    sessionId = session.Id,
                    product_number = session.ProductNumber,
                    price = session.Price,
                    remaining_seats = session.RemainingSeats
                };

                return response200;
            }
        }

    }
}
