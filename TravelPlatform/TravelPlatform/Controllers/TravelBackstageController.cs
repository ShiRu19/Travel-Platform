using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TravelPlatform.Models.Domain;

namespace TravelPlatform.Controllers.v1
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    public class TravelBackstageController : ControllerBase
    {
        private readonly TravelContext _db;

        public TravelBackstageController(TravelContext db) 
        {
            _db = db;
        }

        /// <summary>
        /// Backstage get all travel list
        /// </summary>
        /// <returns>Backstage Travel List Object</returns>
        [MapToApiVersion("1.0")]
        [HttpGet("TravelList")]
        public IActionResult GetTravelList()
        {
            try
            {
                var travelSession = _db.TravelSessions
                                            .Join(_db.Travels,
                                                s => s.TravelId,
                                                t => t.Id,
                                                (s, t) => new
                                                {
                                                    id = s.Id,
                                                    productNumber = s.ProductNumber,
                                                    title = t.Title,
                                                    departure_date = s.DepartureDate,
                                                    days = t.Days,
                                                    price = s.Price,
                                                    remaining_seats = s.RemainingSeats,
                                                    seats = s.Seats,
                                                    group_status = s.GroupStatus
                                                })
                                                .ToList();

                var result = new
                {
                    data = travelSession
                };

                return Ok(travelSession);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
    }
}
