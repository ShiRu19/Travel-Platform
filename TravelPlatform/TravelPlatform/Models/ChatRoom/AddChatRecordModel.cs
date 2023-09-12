namespace TravelPlatform.Models.ChatRoom
{
    public class AddChatRecordModel
    {
        public long roomId { get; set; }
        public int senderId { get; set; }
        public string message { get; set; } = null!;
    }
}
