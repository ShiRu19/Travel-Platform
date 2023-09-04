using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Net;
using TravelPlatform.Models.Domain;
using TravelPlatform.Models.Record;

namespace TravelPlatform.Controllers
{
    public class DomesticGroupStatus
    {
        public int success { get; set; }
        public int failure { get; set; }
    }

    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]

    public class DashboardController : ControllerBase
    {
        private readonly TravelContext _db;
        private IConfiguration _configuration;

        public DashboardController(TravelContext db, IConfiguration configuration)
        {
            _db = db;
            _configuration = configuration;
        }

        /// <summary>
        /// Get sales volume over a date range
        /// </summary>
        /// <param name="nation">To inquire about the nation</param>
        /// <param name="start">Start of date range</param>
        /// <param name="end">End of date range</param>
        /// <returns>Sales volume</returns>
        [MapToApiVersion("1.0")]
        [HttpGet("GetSalesVolume")]
        public IActionResult GetSalesVolume(string nation, DateTime start, DateTime end)
        {
            var salesVolume = _db.Orders
                                .Where(o => o.Nation == nation && o.OrderDate >= start && o.OrderDate <= end)
                                .GroupBy(o => new DateTime(o.OrderDate.Year, o.OrderDate.Month, 1))
                                .Select(s => new
                                {
                                    Month = s.Key.Month,
                                    Count = s.Count()
                                });

            var result = new
            {
                data = salesVolume
            };

            return Ok(result);
        }

        /// <summary>
        /// Get sales over a date range
        /// </summary>
        /// <param name="nation">To inquire about the nation</param>
        /// <param name="start">Start of date range</param>
        /// <param name="end">End of date range</param>
        /// <returns>Sales</returns>
        [MapToApiVersion("1.0")]
        [HttpGet("GetSales")]
        public IActionResult GetSales(string nation, DateTime start, DateTime end)
        {
            var salesVolume = _db.Orders
                                .Where(o => o.Nation == nation && o.OrderDate >= start && o.OrderDate <= end)
                                .GroupBy(o => new DateTime(o.OrderDate.Year, o.OrderDate.Month, 1))
                                .Select(s => new
                                {
                                    Month = s.Key.Month,
                                    Sum = s.Sum(o => o.Total)
                                });

            var result = new
            {
                data = salesVolume
            };

            return Ok(result);
        }

        /// <summary>
        /// Get domestic sales volume over a date range
        /// </summary>
        /// <param name="start">Start of date range</param>
        /// <param name="end">End of date range</param>
        /// <returns>Domestic sales volume</returns>
        [MapToApiVersion("1.0")]
        [HttpGet("GetDomesticSalesVolume")]
        public IActionResult GetDomesticSalesVolume(DateTime start, DateTime end)
        {
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
                                                Order = o.Order,
                                                Nation = t.Nation,
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

                var result = new
                {
                    data = domesticSalesVolume
                };

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        /// <summary>
        /// Get domestic group status over a date range
        /// </summary>
        /// <param name="start">Start of date range</param>
        /// <param name="end">End of date range</param>
        /// <returns>Domestic group status</returns>
        [MapToApiVersion("1.0")]
        [HttpGet("GetDomesticGroupStatus")]
        public IActionResult GetDomesticGroupStatus(DateTime start, DateTime end)
        {
            try
            {
                var domesticGroupStatus = _db.TravelSessions
                                         .Join(_db.Travels,
                                         s => s.TravelId,
                                         t => t.Id,
                                         (s, t) => new
                                         {
                                             TravelSession = s,
                                             Nation = t.Nation,
                                             Location = t.DepartureLocation
                                         })
                                         .Where(s => s.Nation == "台灣" && s.TravelSession.DepartureDate >= start && s.TravelSession.DepartureDate <= end);

                var locations = _configuration.GetSection("Locations").Get<List<string>>();

                if (locations == null)
                {
                    return NotFound();
                }

                var data = new Dictionary<string, DomesticGroupStatus>();
                foreach (var location in locations)
                {
                    var status = new DomesticGroupStatus();
                    status.success = domesticGroupStatus.Where(o => o.Location == location && o.TravelSession.GroupStatus == 1).Count();
                    status.failure = domesticGroupStatus.Where(o => o.Location == location && o.TravelSession.GroupStatus == 0).Count();
                    data[location] = status;
                }

                var result = new
                {
                    data = data
                };

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        /// <summary>
        /// Get top five follow record of open travel
        /// </summary>
        /// <returns></returns>
        [MapToApiVersion("1.0")]
        [HttpGet("GetFollowTopFive")]
        public IActionResult GetFollowTopFive()
        {
            try
            {
                var openTravels = _db.Travels.Where(t => t.DateRangeEnd >= DateTime.Now).ToList();

                var follows = _db.Follows.GroupBy(f => f.TravelId)
                    .Select(f => new
                    {
                        id = f.Key,
                        follows = f.Count()
                    })
                    .OrderByDescending(f => f.follows);

                var data = new List<Object>();

                foreach(var follow in follows)
                {
                    var travel = openTravels.Find(t => t.Id == follow.id);
                    if(travel != null && travel.DateRangeEnd >= DateTime.Now)
                    {
                        var topFollow = new
                        {
                            id = travel.Id,
                            title = travel.Title,
                            nation = travel.Nation == "台灣" ? "國內" : "國外",
                            days = travel.Days,
                            follows = follow.follows
                        };
                        data.Add(topFollow);

                        if(data.Count == 5)
                        {
                            break;
                        }
                    }
                }

                var result = new
                {
                    data = data
                };

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
