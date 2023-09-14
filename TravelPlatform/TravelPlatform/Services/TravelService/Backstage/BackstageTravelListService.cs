using TravelPlatform.Models;
using TravelPlatform.Models.BackstageTravel;
using TravelPlatform.Models.Domain;

namespace TravelPlatform.Services.TravelService.Backstage
{
    public class BackstageTravelListService : IBackstageTravelListService
    {
        private readonly TravelContext _db;
        private readonly IConfiguration _configuration;

        public BackstageTravelListService(TravelContext db, IConfiguration configuration)
        {
            _db = db;
            _configuration = configuration;
        }

        /// <summary>
        /// 取得已關閉行程
        /// </summary>
        /// <returns></returns>
        public async Task<ResponseDto> GetCloseTravelListAsync()
        {
            ResponseDto response200 = new ResponseDto { StatusCode = 200, Message = "", Data = new { } };
            ResponseDto response500 = new ResponseDto { StatusCode = 500, Message = "", Error = "" };

            try
            {
                var travels = _db.Travels.Where(t => t.DateRangeEnd < DateTime.UtcNow)
                    .OrderBy(t => t.DateRangeStart)
                    .Select(t => new
                    {
                        t.Id,
                        t.Title,
                        t.DateRangeStart,
                        t.DateRangeEnd,
                        t.Days,
                        t.Nation,
                        t.DepartureLocation
                    }).ToList();

                response200.Data = new
                {
                    travels
                };

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
        /// 取得已關閉場次
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ResponseDto> GetCloseTravelSessionListAsync(long id)
        {
            ResponseDto response200 = new ResponseDto { StatusCode = 200, Message = "", Data = new { } };
            ResponseDto response500 = new ResponseDto { StatusCode = 500, Message = "", Error = "" };
            ResponseDto response400 = new ResponseDto { StatusCode = 400, Message = "", Error = "" };

            List<TravelSession> travelSession = new List<TravelSession>();
            Models.Domain.Travel travel = new Models.Domain.Travel();

            try
            {
                travelSession = _db.TravelSessions
                        .Where(s => s.TravelId == id && s.DepartureDate < DateTime.UtcNow)
                        .OrderBy(t => t.DepartureDate).ToList();

                var travel_query = _db.Travels.SingleOrDefault(t => t.Id == id);

                if (travel_query == null)
                {
                    response400.StatusCode = 404;
                    response400.Error = _configuration["ErrorMessage:NOT_FOUND"];
                    response400.Message = "There is an travel does not exist.";
                    return response400;
                }

                travel = travel_query;
            }
            catch (Exception ex)
            {
                response500.Error = _configuration["ErrorMessage:DB_OP_EX"];
                response500.Message = ex.Message;
                return response500;
            }

            var data = new List<TravelSessionList>();
            foreach (var session in travelSession)
            {
                var sessionData = new TravelSessionList()
                {
                    Id = session.Id,
                    ProductNumber = session.ProductNumber,
                    Title = travel.Title,
                    departureDate = session.DepartureDate.ToString("yyyy/MM/dd"),
                    Days = travel.Days,
                    Price = session.Price,
                    RemainingSeats = session.RemainingSeats,
                    Seats = session.Seats,
                    GroupStatus = session.GroupStatus
                };

                data.Add(sessionData);
            }

            response200.Data = data;

            return response200;
        }

        /// <summary>
        /// 取得開放中行程
        /// </summary>
        /// <returns></returns>
        public async Task<ResponseDto> GetOpenTravelListAsync()
        {
            ResponseDto response200 = new ResponseDto { StatusCode = 200, Message = "", Data = new { } };
            ResponseDto response500 = new ResponseDto { StatusCode = 500, Message = "", Error = "" };

            try
            {
                var travels = _db.Travels.Where(t => t.DateRangeEnd >= DateTime.UtcNow)
                    .OrderBy(t => t.DateRangeStart)
                    .Select(t => new
                    {
                        t.Id,
                        t.Title,
                        t.DateRangeStart,
                        t.DateRangeEnd,
                        t.Days,
                        t.Nation,
                        t.DepartureLocation
                    }).ToList();

                response200.Data = new
                {
                    travels
                };

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
        /// 取得開放中場次
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ResponseDto> GetOpenTravelSessionListAsync(long id)
        {
            ResponseDto response200 = new ResponseDto { StatusCode = 200, Message = "", Data = new { } };
            ResponseDto response500 = new ResponseDto { StatusCode = 500, Message = "", Error = "" };
            ResponseDto response400 = new ResponseDto { StatusCode = 400, Message = "", Error = "" };

            List<TravelSession> travelSession = new List<TravelSession>();
            Models.Domain.Travel travel = new Models.Domain.Travel();

            try
            {
                travelSession = _db.TravelSessions
                        .Where(s => s.TravelId == id && s.DepartureDate >= DateTime.UtcNow)
                        .OrderBy(t => t.DepartureDate).ToList();
                
                var travel_query = _db.Travels.SingleOrDefault(t => t.Id == id);
                
                if (travel_query == null)
                {
                    response400.StatusCode = 404;
                    response400.Error = _configuration["ErrorMessage:NOT_FOUND"];
                    response400.Message = "There is an travel does not exist.";
                    return response400;
                }

                travel = travel_query;
            }
            catch (Exception ex)
            {
                response500.Error = _configuration["ErrorMessage:DB_OP_EX"];
                response500.Message = ex.Message;
                return response500;
            }

            var data = new List<TravelSessionList>();
            foreach (var session in travelSession)
            {
                var sessionData = new TravelSessionList()
                {
                    Id = session.Id,
                    ProductNumber = session.ProductNumber,
                    Title = travel.Title,
                    departureDate = session.DepartureDate.ToString("yyyy/MM/dd"),
                    Days = travel.Days,
                    Price = session.Price,
                    RemainingSeats = session.RemainingSeats,
                    Seats = session.Seats,
                    GroupStatus = session.GroupStatus
                };

                data.Add(sessionData);
            }

            response200.Data = data;

            return response200;
        }
    }
}
