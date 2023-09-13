using TravelPlatform.Models;
using TravelPlatform.Models.Domain;

namespace TravelPlatform.Services.DashboardService
{
    public class DomesticGroupStatus
    {
        public int success { get; set; }
        public int failure { get; set; }
    }
    
    public class DashboardService : IDashboardService
    {
        private readonly TravelContext _db;
        private readonly IConfiguration _configuration;

        public DashboardService(TravelContext db, IConfiguration configuration)
        {
            _db = db;
            _configuration = configuration;
        }

        /// <summary>
        /// 取得各出發地成團狀態
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        public async Task<ResponseDto> GetDomesticGroupStatusAsync(DateTime start, DateTime end)
        {
            ResponseDto response200 = new ResponseDto { StatusCode = 200, Message = "", Data = new { } };
            ResponseDto response500 = new ResponseDto { StatusCode = 500, Message = "", Error = "" };
            ResponseDto response400 = new ResponseDto { StatusCode = 400, Message = "", Error = "" };

            try
            {
                var domesticGroupStatus = _db.TravelSessions
                                             .Join(_db.Travels,
                                             s => s.TravelId,
                                             t => t.Id,
                                             (s, t) => new
                                             {
                                                 TravelSession = s,
                                                 t.Nation,
                                                 Location = t.DepartureLocation
                                             })
                                             .Where(s => s.Nation == "台灣" && s.TravelSession.DepartureDate >= start && s.TravelSession.DepartureDate <= end);

                var locations = _configuration.GetSection("Locations").Get<List<string>>();

                if (locations == null)
                {
                    response400.StatusCode = 404;
                    response400.Error = _configuration["ErrorMessage:NOT_FOUND"];
                    response400.Message = "Location does not found";
                    return response400;
                }
                
                var data = new Dictionary<string, DomesticGroupStatus>();
                foreach (var location in locations)
                {
                    var status = new DomesticGroupStatus();
                    status.success = domesticGroupStatus.Where(o => o.Location == location && o.TravelSession.GroupStatus == 1).Count();
                    status.failure = domesticGroupStatus.Where(o => o.Location == location && o.TravelSession.GroupStatus == 0).Count();
                    data[location] = status;
                }
                
                response200.Data = data;
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
        /// 取得各地區訂單量
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        public async Task<ResponseDto> GetDomesticSalesVolumeAsync(DateTime start, DateTime end)
        {
            ResponseDto response200 = new ResponseDto { StatusCode = 200, Message = "", Data = new { } };
            ResponseDto response500 = new ResponseDto { StatusCode = 500, Message = "", Error = "" };

            try
            {
                var domesticSalesVolume = _db.Orders
                                            .Join(_db.TravelSessions,
                                            o => o.TravelSessionId,
                                            s => s.Id,
                                            (o, s) => new
                                            {
                                                Order = o,
                                                TravelSession = s
                                            })
                                            .Join(_db.Travels,
                                            o => o.TravelSession.TravelId,
                                            t => t.Id,
                                            (o, t) => new
                                            {
                                                o.Order,
                                                t.Nation,
                                                Location = t.DepartureLocation
                                            })
                                            .Where(o => o.Order.OrderDate >= start && o.Order.OrderDate <= end && o.Nation == "台灣")
                                            .GroupBy(s => s.Location)
                                            .Select(s => new
                                            {
                                                Location = s.Key,
                                                Count = s.Count()
                                            })
                                            .ToList();

                response200.Data = domesticSalesVolume;
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
        /// 取得前五高的追蹤行程
        /// </summary>
        /// <returns></returns>
        public async Task<ResponseDto> GetFollowTopFiveAsync()
        {
            ResponseDto response200 = new ResponseDto { StatusCode = 200, Message = "", Data = new { } };
            ResponseDto response500 = new ResponseDto { StatusCode = 500, Message = "", Error = "" };

            try
            {
                var openTravels = _db.Travels.Where(t => t.DateRangeEnd >= DateTime.Now).ToList();

                var follows = _db.Follows.GroupBy(f => f.TravelId)
                    .Select(f => new
                    {
                        id = f.Key,
                        follows = f.Count()
                    })
                    .OrderByDescending(f => f.follows)
                    .ToList();
                
                var data = new List<Object>();

                foreach (var follow in follows)
                {
                    var travel = openTravels.Find(t => t.Id == follow.id);
                    if (travel != null && travel.DateRangeEnd >= DateTime.Now)
                    {
                        var topFollow = new
                        {
                            id = travel.Id,
                            title = travel.Title,
                            nation = travel.Nation == "台灣" ? "國內" : "國外",
                            days = travel.Days,
                            follow.follows
                        };
                        data.Add(topFollow);

                        if (data.Count == 5) break;
                    }
                }

                response200.Data = data;
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
        /// 取得各月份銷售總額
        /// </summary>
        /// <param name="nation"></param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        public async Task<ResponseDto> GetSalesAsync(string nation, DateTime start, DateTime end)
        {
            ResponseDto response200 = new ResponseDto { StatusCode = 200, Message = "", Data = new { } };
            ResponseDto response500 = new ResponseDto { StatusCode = 500, Message = "", Error = "" };

            try
            {
                var salesVolume = _db.Orders
                                    .Where(o => o.Nation == nation && o.OrderDate >= start && o.OrderDate <= end)
                                    .GroupBy(o => new DateTime(o.OrderDate.Year, o.OrderDate.Month, 1))
                                    .Select(s => new
                                    {
                                        s.Key.Month,
                                        Sum = s.Sum(o => o.Total)
                                    });

                response200.Data = salesVolume;

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
        /// 取得各月份訂單量
        /// </summary>
        /// <param name="nation"></param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        public async Task<ResponseDto> GetSalesVolumeAsync(string nation, DateTime start, DateTime end)
        {
            ResponseDto response200 = new ResponseDto { StatusCode = 200, Message = "", Data = new { } };
            ResponseDto response500 = new ResponseDto { StatusCode = 500, Message = "", Error = "" };

            try
            {
                var salesVolume = _db.Orders
                                    .Where(o => o.Nation == nation && o.OrderDate >= start && o.OrderDate <= end)
                                    .GroupBy(o => new DateTime(o.OrderDate.Year, o.OrderDate.Month, 1))
                                    .Select(s => new
                                    {
                                        s.Key.Month,
                                        Count = s.Count()
                                    });

                response200.Data = salesVolume;

                return response200;
            }
            catch (Exception ex)
            {
                response500.Error = _configuration["ErrorMessage:DB_OP_EX"];
                response500.Message = ex.Message;

                return response500;
            }
        }
    }
}
