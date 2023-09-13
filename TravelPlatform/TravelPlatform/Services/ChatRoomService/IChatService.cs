using Microsoft.AspNetCore.Mvc;
using TravelPlatform.Models;
using TravelPlatform.Models.ChatRoom;

namespace TravelPlatform.Services.ChatRoom
{
    public interface IChatService
    {
        Task<ResponseDto> SaveChatMessageAsync(AddChatRecordModel addChatRecordModel);
        Task<ResponseDto> GetChatRoomListAsync();
        Task<ResponseDto> GetChatRecordAsync(long roomId);
    }
}
