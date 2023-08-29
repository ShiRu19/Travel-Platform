using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TravelPlatform.Models.Domain;
using TravelPlatform.Services;

namespace TravelPlatform.Controllers
{
    public class AddChatRecordModel
    {
        public long roomId { get; set; }
        public int senderId { get; set; }
        public string message { get; set; } = null!;
    }

    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    public class ChatController : ControllerBase
    {
        private readonly TravelContext _db;
        public ChatController(TravelContext db)
        {
            _db = db;
        }

        [MapToApiVersion("1.0")]
        [HttpGet("GetChatRecord")]
        public async Task<IActionResult> GetChatRecord(long roomId)
        {
            var record = _db.Chats.Where(c => c.RoomId == roomId)
                .OrderBy(c => c.SendTime)
                .Select(c => new
                {
                    c.Sender,
                    c.Message,
                    SendTime = c.SendTime.ToString("g")
                })
                .ToList();

            return Ok(new
            {
                record
            });
        }

        [MapToApiVersion("1.0")]
        [HttpGet("GetChatRoomList")]
        public async Task<IActionResult> GetChatRoomList()
        {
            var chatRoomList = _db.Chats.Select(c => c.RoomId).Distinct().ToList();
            return Ok(chatRoomList);
        }

        [MapToApiVersion("1.0")]
        [HttpPost("SaveChatMessage")]
        public async Task<IActionResult> SaveChatMessage(AddChatRecordModel addChatRecordModel)
        {
            using(var transaction = _db.Database.BeginTransaction())
            {
                try
                {
                    Chat newChat = new Chat
                    {
                        Id = _db.Chats.Count() == 0 ? 1 : _db.Chats.Max(c => c.Id) + 1,
                        RoomId = addChatRecordModel.roomId,
                        Sender = addChatRecordModel.senderId,
                        Message = addChatRecordModel.message,
                        SendTime = DateTime.Now
                    };

                    _db.Chats.Add(newChat);
                    _db.SaveChanges();
                    transaction.Commit();
                    Console.WriteLine("Transaction committed successfully.");
                    return Ok();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();

                    Console.WriteLine("Transaction rolled back due to an error: " + ex.Message);
                    return StatusCode(500, ex.Message);
                }
            }

        }
    }
}
