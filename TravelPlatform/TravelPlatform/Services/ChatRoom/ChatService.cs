using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;
using TravelPlatform.Models;
using TravelPlatform.Models.ChatRoom;
using TravelPlatform.Models.Domain;

namespace TravelPlatform.Services.ChatRoom
{
    public class ChatService : IChatService
    {
        private readonly TravelContext _db;
        private readonly IConfiguration _configuration;

        public ChatService(TravelContext db, IConfiguration configuration)
        {
            _db = db;
            _configuration = configuration;
        }

        /// <summary>
        /// 儲存聊天紀錄
        /// </summary>
        /// <param name="addChatRecordModel"></param>
        /// <returns></returns>
        public async Task<ResponseDto> SaveChatMessageAsync(AddChatRecordModel addChatRecordModel)
        {
            ResponseDto response200 = new ResponseDto() { StatusCode = 200 };
            ResponseDto response500 = new ResponseDto() { StatusCode = 500 };

            using (var transaction = _db.Database.BeginTransaction())
            {
                Chat newChat = new Chat();
                try
                {
                    newChat.Id = _db.Chats.Count() == 0 ? 1 : _db.Chats.Max(c => c.Id) + 1;
                    newChat.RoomId = addChatRecordModel.roomId;
                    newChat.Sender = addChatRecordModel.senderId;
                    newChat.Message = addChatRecordModel.message;
                    newChat.SendTime = DateTime.UtcNow;
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    Console.WriteLine("Transaction rolled back due to an error: " + ex.Message);

                    response500.Error = _configuration["ErrorMessage:DB_OP_EX"];
                    response500.Message = ex.Message;
                    
                    return response500;
                }

                try
                {
                    _db.Chats.Add(newChat);
                    _db.SaveChanges();
                    transaction.Commit();
                    Console.WriteLine("Transaction committed successfully.");

                    return response200;
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    Console.WriteLine("Transaction rolled back due to an error: " + ex.Message);

                    response500.Error = _configuration["ErrorMessage:DB_SAVE_EX"];
                    response500.Message = ex.Message;
                    
                    return response500;
                }
            }
        }

        /// <summary>
        /// 取得聊天室列表
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<ResponseDto> GetChatRoomListAsync()
        {
            ResponseDto response200 = new ResponseDto() { StatusCode = 200 };
            ResponseDto response500 = new ResponseDto() { StatusCode = 500 };

            var chatRoomList = new List<long>();
            using(var transaction = _db.Database.BeginTransaction(IsolationLevel.ReadCommitted))
            {
                try
                {
                    chatRoomList = _db.Chats.Select(c => c.RoomId).Distinct().ToList();
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    Console.WriteLine("Transaction rolled back due to an error: " + ex.Message);

                    response500.Error = _configuration["ErrorMessage:DB_OP_EX"];
                    response500.Message = ex.Message;
                    
                    return response500;
                }
            }

            var chatUserRoomList = new List<ChatUserRoomModel>();
            foreach (var chatRoom in chatRoomList)
            {
                ChatUserRoomModel chatUserRoom = new ChatUserRoomModel();

                var user = _db.Users.SingleOrDefault(u => u.Id == chatRoom);
                if (user != null)
                {
                    chatUserRoom.RoomId = chatRoom;
                    chatUserRoom.UserName = user.Name;
                    chatUserRoomList.Add(chatUserRoom);
                }
            }

            response200.Data = chatUserRoomList;

            return response200;
        }

        /// <summary>
        /// 取得聊天歷史紀錄
        /// </summary>
        /// <param name="roomId"></param>
        /// <returns></returns>
        public async Task<ResponseDto> GetChatRecordAsync(long roomId)
        {
            ResponseDto response200 = new ResponseDto() { StatusCode = 200 };
            ResponseDto response500 = new ResponseDto() { StatusCode = 500 };

            using (var transaction = _db.Database.BeginTransaction(IsolationLevel.ReadCommitted))
            {
                try
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

                    transaction.Commit();
                    response200.Data = record;
                    return response200;
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    Console.WriteLine("Transaction rolled back due to an error: " + ex.Message);

                    response500.Error = _configuration["ErrorMessage:DB_OP_EX"];
                    response500.Message = ex.Message;
                    return response500;
                }
            }
        }
    }
}
