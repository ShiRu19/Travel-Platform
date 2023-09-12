using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Sprache;
using TravelPlatform.Models;
using TravelPlatform.Models.ChatRoom;
using TravelPlatform.Models.Domain;
using TravelPlatform.Services;
using TravelPlatform.Services.ChatRoom;
using TravelPlatform.Services.Response;

namespace TravelPlatform.Controllers
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    public class ChatController : ControllerBase
    {
        private readonly TravelContext _db;
        private readonly IChatService _chatService;
        private readonly IResponseService _responseService;
        public ChatController(TravelContext db, IChatService chatService, IResponseService responseService)
        {
            _db = db;
            _chatService = chatService;
            _responseService = responseService;
        }

        [MapToApiVersion("1.0")]
        [HttpGet("GetChatRecord")]
        public async Task<IActionResult> GetChatRecord(long roomId)
        {
            var response = await _chatService.GetChatRecordAsync(roomId);
            return _responseService.ReturnResponse(response);
        }

        [MapToApiVersion("1.0")]
        [HttpGet("GetChatRoomList")]
        public async Task<IActionResult> GetChatRoomList()
        {
            var response = await _chatService.GetChatRoomListAsync();
            return _responseService.ReturnResponse(response);
        }

        [MapToApiVersion("1.0")]
        [HttpPost("SaveChatMessage")]
        public async Task<IActionResult> SaveChatMessage(AddChatRecordModel addChatRecordModel)
        {
            var response = await _chatService.SaveChatMessageAsync(addChatRecordModel);
            return _responseService.ReturnResponse(response);
        }
    }
}
