using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TravelPlatform.Models.Domain;
using TravelPlatform.Services;

namespace TravelPlatform.Controllers
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    public class UserController : ControllerBase
    {
        private readonly TravelContext _db;

        public UserController(TravelContext db)
        {
            _db = db;
        }

        [MapToApiVersion("1.0")]
        [HttpGet("GetUserList")]
        public IActionResult GetUserList()
        {
            try
            {
                var user = _db.Users.Select(u => new
                {
                    id = u.Id,
                    name = u.Name,
                    sex = u.Sex,
                    birthday = u.Birthday,
                    email = u.Email,
                    phoneNumber = u.PhoneNumber,
                    region = u.Region
                });
                return Ok(user);
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
