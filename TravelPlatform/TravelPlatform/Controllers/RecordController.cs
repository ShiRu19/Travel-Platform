using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TravelPlatform.Handler.Response;
using TravelPlatform.Hubs;
using TravelPlatform.Models.Domain;
using TravelPlatform.Models.Record;
using TravelPlatform.Services.Record;

namespace TravelPlatform.Controllers
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    [Authorize]
    public class RecordController : ControllerBase
    {
        private readonly TravelContext _db;
        private readonly IFollowService _followService;
        private readonly IResponseHandler _responseHandler;

        public RecordController(TravelContext db, IFollowService followService, IResponseHandler responseHandler)
        {
            _db = db;
            _followService = followService;
            _responseHandler = responseHandler;
        }

        /// <summary>
        /// User follow this travel
        /// </summary>
        /// <param name="followModel"></param>
        /// <returns></returns>
        [MapToApiVersion("1.0")]
        [HttpPost("AddFollow")]
        public async Task<IActionResult> AddFollow([FromBody] FollowModel followModel)
        {
            var response = await _followService.AddFollowAsync(followModel);
            return _responseHandler.ReturnResponse(response);
        }

        /// <summary>
        /// User unfollow this travel
        /// </summary>
        /// <param name="followModel"></param>
        /// <returns></returns>
        [MapToApiVersion("1.0")]
        [HttpPost("CancelFollow")]
        public async Task<IActionResult> CancelFollow([FromBody] FollowModel followModel)
        {
            var response = await _followService.CancelFollowAsync(followModel);
            return _responseHandler.ReturnResponse(response);
        }

        /// <summary>
        /// Comfirm whether the user is following this travel
        /// </summary>
        /// <param name="TravelId"></param>
        /// <param name="UserId"></param>
        /// <returns></returns>
        [MapToApiVersion("1.0")]
        [HttpGet("CheckFollow")]
        public async Task<IActionResult> CheckFollow(long TravelId, long UserId)
        {
            FollowModel follow = new FollowModel()
            {
                TravelId = TravelId,
                UserId = UserId
            };
            var response = await _followService.CheckFollowAsync(follow);
            return _responseHandler.ReturnResponse(response);
        }

        /// <summary>
        /// Get following travel list for user
        /// </summary>
        /// <param name="UserId"></param>
        /// <returns></returns>
        [MapToApiVersion("1.0")]
        [HttpGet("GetUserFollowList")]
        public async Task<IActionResult> GetUserFollowList(long UserId)
        {
            var response = await _followService.GetUserFollowListAsync(UserId);
            return _responseHandler.ReturnResponse(response);
        }
    }
}