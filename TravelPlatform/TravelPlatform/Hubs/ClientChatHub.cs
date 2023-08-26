
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
        public static List<string> UsersId = new List<string>();
        public static Dictionary<string, int> UserCount = new Dictionary<string, int>();

        /// <summary>
        /// 連線事件
        /// </summary>
        /// <returns></returns>
        public override async Task OnConnectedAsync()
        {
            User user = new User
            {
                Id = Guid.NewGuid().ToString(),
                ConnectionId = Context.ConnectionId
            };

            Room room = new Room();
            room.Id = Guid.NewGuid().ToString();

            Rooms.Add(room);
            Users.Add(user);
            RoomsId.Add(room.Id);
            UsersId.Add(user.Id);
            UserCount[room.Id] = room.Users.Count;

            // 更新連線 Room ID
            await Clients.Client(Context.ConnectionId).SendAsync("YourRoomID", room.Id);

            // 更新連線 User ID
            await Clients.Client(Context.ConnectionId).SendAsync("YourUserID", user.Id);

            // 更新連線 Room ID 列表
            string jsonString_room = JsonConvert.SerializeObject(RoomsId);
            await Clients.All.SendAsync("UpdRooms", jsonString_room);

            // 更新連線 User ID 列表
            string jsonString_user = JsonConvert.SerializeObject(UsersId);
            await Clients.All.SendAsync("UpdUsers", jsonString_user);

            // 更新各房間人數
            string jsonString_count = JsonConvert.SerializeObject(UserCount);
            await Clients.All.SendAsync("UpdUserCount", jsonString_count);

            await base.OnConnectedAsync();
            JoinGroup(room.Id);
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
                UserCount[room.Id] = room.Users.Count;

                if (room.Users.Count == 0)
                {
                    RoomsId.Remove(room.Id);
                    Rooms.Remove(room);
                    UserCount.Remove(room.Id);
                }
            });

            Users.Remove(user);
            UsersId.Remove(user.Id);

            // 更新連線 Room ID 列表
            string jsonString_room = JsonConvert.SerializeObject(RoomsId);
            await Clients.All.SendAsync("UpdRooms", jsonString_room);

            // 更新連線 User ID 列表
            string jsonString_user = JsonConvert.SerializeObject(UsersId);
            await Clients.All.SendAsync("UpdUsers", jsonString_user);

            // 更新各房間人數
            string jsonString_count = JsonConvert.SerializeObject(UserCount);
            await Clients.All.SendAsync("UpdUserCount", jsonString_count);

            await base.OnDisconnectedAsync(ex);
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
            if (room != null)
            {
                await Groups.AddToGroupAsync(Context.ConnectionId, roomId);

                room.Users.Add(user);

                UserCount[room.Id] = room.Users.Count;
            }

            // 更新連線 Room ID
            await Clients.Client(Context.ConnectionId).SendAsync("YourRoomID", room.Id);

            // 更新各房間人數
            string jsonString_count = JsonConvert.SerializeObject(UserCount);
            await Clients.All.SendAsync("UpdUserCount", jsonString_count);
        }

        /// <summary>
        /// 傳遞訊息
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        public async Task SendMessage(string roomId, string userId, string msg)
        {
            await Clients.Group(roomId).SendAsync("UpdContent", userId, msg);
        }
    }
}
