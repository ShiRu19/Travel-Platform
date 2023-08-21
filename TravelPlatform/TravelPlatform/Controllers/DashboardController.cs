using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using TravelPlatform.Models.Domain;

namespace TravelPlatform.Controllers
{
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

        [MapToApiVersion("1.0")]
        [HttpGet("GetSalesVolume")]
        public IActionResult GetSalesVolume(string nation, DateTime start, DateTime end)
        {
            var salesVolume = _db.Orders
                                .Where(o => o.Nation == nation && o.OrderDate >= start && o.OrderDate <= end)
                                .GroupBy(o => new DateTime(o.OrderDate.Year, o.OrderDate.Month, 1))
                                .Select(s => new
                                {
                                    Month = s.Key,
                                    Count = s.Count(),
                                    Sum = s.Sum(o => o.Total)
                                });

            var result = new
            {
                data = salesVolume
            };

            return Ok(result);
        }

        [MapToApiVersion("1.0")]
        [HttpGet("GetDomesticSalesVolume")]
        public IActionResult GetDomesticSalesVolume(DateTime start, DateTime end)
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
                                        .Where(o => o.Order.OrderDate >= start && o.Order.OrderDate <= end && o.Nation == "台灣");

            var locations = _configuration.GetSection("Locations").Get<List<string>>();

            if (locations == null)
            {
                return NotFound();
            }

            var result = new Dictionary<string, int>();
            foreach (var location in locations)
            {
                result[location] = domesticSalesVolume.Where(o => o.Location == location).Count();
            }

            return Ok(result);
        }
    }
}
