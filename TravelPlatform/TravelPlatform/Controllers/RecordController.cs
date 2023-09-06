using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TravelPlatform.Models.Domain;
using TravelPlatform.Models.Record;

namespace TravelPlatform.Controllers
{

    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    [Authorize]
    public class RecordController : ControllerBase
    {
        private readonly TravelContext _db;

        public RecordController(TravelContext db)
        {
            _db = db;
        }

        /// <summary>
        /// User follow this travel
        /// </summary>
        /// <param name="followModel"></param>
        /// <returns></returns>
        [MapToApiVersion("1.0")]
        [HttpPost("AddFollow")]
        public IActionResult AddFollow([FromBody] FollowModel followModel)
        {
            try
            {
                var follow = _db.Follows.Where(f => f.UserId == followModel.UserId && f.TravelId == followModel.TravelId).FirstOrDefault();
                
                if(follow != null)
                {
                    return BadRequest(new
                    {
                        error = "Follow record is already exist.",
                        message = "Please confirm whether the user has already followed this travel."
                    });
                }

                var addFollow = new Follow()
                {
                    Id = _db.Follows.Count() == 0 ? 1 : _db.Follows.Max(f => f.Id) + 1,
                    TravelId = followModel.TravelId,
                    UserId = followModel.UserId
                };

                _db.Follows.Add(addFollow);
                _db.SaveChanges();
                return Ok(addFollow);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        /// <summary>
        /// User unfollow this travel
        /// </summary>
        /// <param name="followModel"></param>
        /// <returns></returns>
        [MapToApiVersion("1.0")]
        [HttpPost("CancelFollow")]
        public IActionResult CancelFollow([FromBody] FollowModel followModel)
        {
            try
            {
                var follow = _db.Follows.Where(f => f.UserId == followModel.UserId && f.TravelId == followModel.TravelId).FirstOrDefault();

                if (follow == null)
                {
                    return BadRequest(new
                    {
                        error = "Follow record is not found.",
                        message = "Please confirm whether the user has already followed this travel."
                    });
                }

                _db.Follows.Remove(follow);
                _db.SaveChanges();
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        /// <summary>
        /// Comfirm whether the user is following this travel
        /// </summary>
        /// <param name="TravelId"></param>
        /// <param name="UserId"></param>
        /// <returns></returns>
        [MapToApiVersion("1.0")]
        [HttpGet("CheckFollow")]
        public IActionResult CheckFollow(long TravelId, long UserId)
        {
            try
            {
                var follow = _db.Follows.Where(f => f.UserId == UserId && f.TravelId == TravelId).FirstOrDefault();

                if (follow == null)
                {
                    return BadRequest(new
                    {
                        error = "Follow record is not found.",
                        message = "Please confirm whether the user has already followed this travel."
                    });
                }

                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        /// <summary>
        /// Get following travel list
        /// </summary>
        /// <param name="UserId"></param>
        /// <returns></returns>
        [MapToApiVersion("1.0")]
        [HttpGet("GetFollowList")]
        public IActionResult GetFollowList(long UserId)
        {
            try
            {
                var follow_all = _db.Follows.Where(f => f.UserId == UserId).ToList();
                
                var followList_open = new List<Travel>();
                var followList_close = new List<Travel>();

                foreach (var follow in follow_all)
                {
                    var travel = _db.Travels.Where(t => t.Id == follow.TravelId).First();

                    if(travel.DateRangeEnd < DateTime.UtcNow)
                    {
                        followList_close.Add(travel);
                    }
                    else
                    {
                        followList_open.Add(travel);
                    }
                }

                return Ok(new
                {
                    open = followList_open,
                    close = followList_close
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

    }
}
