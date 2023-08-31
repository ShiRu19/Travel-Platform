using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TravelPlatform.Models.Domain;
using TravelPlatform.Services;

namespace TravelPlatform.Controllers
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    public class ForestageTravelController : ControllerBase
    {
        private readonly TravelContext _db;

        public ForestageTravelController(TravelContext db)
        {
            _db = db;
        }

        /// <summary>
        /// Get travel list
        /// </summary>
        /// <returns>Travels list</returns>
        [MapToApiVersion("1.0")]
        [HttpGet("GetTravelList")]
        public IActionResult GetTravelList()
        {
            try
            {
                var travels = _db.Travels.Where(t => t.DateRangeEnd >= DateTime.Now)
                    .Select(t => new
                    {
                        t.Id,
                        t.Title,
                        DateRangeStart = t.DateRangeStart.ToString("yyyy-MM-dd"),
                        DateRangeEnd = t.DateRangeEnd.ToString("yyyy-MM-dd"),
                        t.MainImageUrl
                    }).ToList();

                var result = new
                {
                    data = travels
                };

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        /// <summary>
        /// Get travel detail
        /// </summary>
        /// <param name="id">travel id</param>
        /// <returns>Travel detail</returns>
        [MapToApiVersion("1.0")]
        [HttpGet("GetTravelDetail")]
        public IActionResult GetTravelDetail(long id)
        {
            try
            {
                var travel = _db.Travels.Where(t => t.Id == id)
                    .Select(t => new
                    {
                        t.Title,
                        DateRangeStart = t.DateRangeStart.ToString("d"),
                        DateRangeEnd = t.DateRangeEnd.ToString("d"),
                        t.Nation,
                        t.PdfUrl,
                        t.MainImageUrl
                    });

                var travelSessions = _db.TravelSessions.Where(t => t.TravelId == id && t.DepartureDate >= DateTime.Now)
                    .Select(t => new
                    {
                        t.ProductNumber,
                        DepartureDate = t.DepartureDate.ToString("d") + "(" + t.DepartureDate.ToString("ddd").Substring(1) + ")",
                        t.RemainingSeats,
                        t.Seats,
                        GroupStatus = t.GroupStatus == 1 ? "已成團" : "尚未成團",
                        Price = t.Price.ToString("N0")
                    });

                var result = new
                {
                    data = new
                    {
                        travelInfo = travel,
                        travelSessions = travelSessions
                    }
                };

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        /// <summary>
        /// Get session detail
        /// </summary>
        /// <param name="productNumber"></param>
        /// <returns></returns>
        [MapToApiVersion("1.0")]
        [HttpGet("GetSessionDetail")]
        public IActionResult GetSessionDetail(string productNumber)
        {
            try
            {
                var session = _db.TravelSessions.Where(t => t.ProductNumber == productNumber).FirstOrDefault();
                if (session == null)
                {
                    return BadRequest(new
                    {
                        error = "Session id is not found.",
                        message = "Please confirm session id."
                    });
                }

                var travel = _db.Travels.Where(t => t.Id == session.TravelId).First();

                var startDate = session.DepartureDate;
                var endDate = session.DepartureDate.AddDays(travel.Days - 1);

                return Ok(new
                {
                    title = travel.Title,
                    departure_date_start = startDate + "(" + startDate.ToString("ddd").Substring(1) + ")",
                    departure_date_end = endDate.ToString("d") + "(" + endDate.ToString("ddd").Substring(1) + ")",
                    days = travel.Days,
                    product_number = session.ProductNumber,
                    price = session.Price,
                    remaining_seats = session.RemainingSeats,
                });
            }
            catch(Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
