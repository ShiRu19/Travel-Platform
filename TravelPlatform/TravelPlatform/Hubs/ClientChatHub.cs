
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using System.Numerics;
using TravelPlatform.Models.Domain;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace TravelPlatform.Hubs
{
    public class Room
    {
        public string Id { get; set; } = null!;
        public List<User> Users { get; set; } = new List<User>();
    }

    public class User
    {
        public string Id { get; set; } = null!;
        public string ConnectionId { get; set; } = null!;
    }

    public class ClientChatHub : Hub
    {
        public static List<Room> Rooms = new List<Room>();
        public static List<User> Users = new List<User>();
        public static List<string> RoomsId = new List<string>();

        /// <summary>
        /// 連線事件 / 創建使用者
        /// </summary>
        /// <returns></returns>
        public override async Task OnConnectedAsync()
        {
            User user = new User
            {
                Id = Guid.NewGuid().ToString(),
                ConnectionId = Context.ConnectionId
            };

            Users.Add(user);

            // 更新連線 User ID
            await Clients.Client(Context.ConnectionId).SendAsync("YourUserID", user.Id);

            await base.OnConnectedAsync();
        }

        /// <summary>
        /// 創建聊天室
        /// </summary>
        /// <param name="roomId"></param>
        /// <returns></returns>
        public async Task CreateRoom(string roomId)
        {
            Room room = new Room();
            room.Id = roomId;

            Rooms.Add(room);
            RoomsId.Add(room.Id);

            //// 更新連線 Room ID 列表
            //string jsonString_room = JsonConvert.SerializeObject(RoomsId);
            //await Clients.All.SendAsync("UpdRooms", jsonString_room);
        }

        /// <summary>
        /// 加入聊天室
        /// </summary>
        /// <param name="roomId"></param>
        /// <returns></returns>
        public async Task JoinGroup(string roomId)
        {
            var user = Users.Where(u => u.ConnectionId == Context.ConnectionId).First();
            var room = Rooms.Where(r => r.Id == roomId).FirstOrDefault();

            if (room == null)
            {
                CreateRoom(roomId);
                room = Rooms.Where(r => r.Id == roomId).FirstOrDefault();
            }

            await Groups.AddToGroupAsync(Context.ConnectionId, roomId);

            room.Users.Add(user);

            // 更新連線 Room ID
            await Clients.Client(Context.ConnectionId).SendAsync("YourRoomID", room.Id);
        }

        /// <summary>
        /// 傳遞訊息
        /// </summary>
        /// <param name="roomId"></param>
        /// <param name="userId"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public async Task SendMessage(string roomId, string userId, string msg)
        {
            await Clients.Group(roomId).SendAsync("UpdContent", userId, msg);
        }

        /// <summary>
        /// 離線事件
        /// </summary>
        /// <param name="ex"></param>
        /// <returns></returns>
        public override async Task OnDisconnectedAsync(Exception ex)
        {
            User user = Users.Where(u => u.ConnectionId == Context.ConnectionId).First();

            var rooms = Rooms.Where(r => r.Users.Exists(u => u.ConnectionId == Context.ConnectionId)).ToList();

            rooms.ForEach((room) =>
            {
                room.Users.Remove(user);
            });

            Users.Remove(user);

            await base.OnDisconnectedAsync(ex);
        }
    }
}
