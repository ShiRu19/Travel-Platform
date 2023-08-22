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

        [MapToApiVersion("1.0")]
        [HttpGet("GetTravelList")]
        public IActionResult GetTravelList()
        {
            try
            {
                var travels = _db.Travels.Select(t => new
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

    }
}
