using System.Collections.Concurrent;
using System.Net.WebSockets;
using System.Text;
using TravelPlatform.Models.Domain;

namespace TravelPlatform.Handler;

public class WebSocketManager
{
    private readonly ConcurrentDictionary<string, ConcurrentDictionary<Guid, WebSocket>> _channelSockets
       = new ConcurrentDictionary<string, ConcurrentDictionary<Guid, WebSocket>>();

    public async Task<Guid> AddSocketAsync(string channelName, WebSocket socket)
    {
        var socketId = Guid.NewGuid();

        _channelSockets.AddOrUpdate(channelName,
                new ConcurrentDictionary<Guid, WebSocket> { [socketId] = socket },
                (_, existingDict) => {
                    existingDict[socketId] = socket;
                    return existingDict;
                });

        return socketId;
    }

    public async Task RemoveSocketAsync(string channelName, Guid socketId)
    {
        if (_channelSockets.TryGetValue(channelName, out var socketDict))
        {
            socketDict.TryRemove(socketId, out _);
        }
    }

    public async Task BroadcastToChannelAsync(string channelName, string message)
    {
        var serverMsg = Encoding.UTF8.GetBytes(message);

        if (_channelSockets.TryGetValue(channelName, out var channelSockets))
        {
            foreach (var socket in channelSockets.Values)
            {
                if (socket.State == WebSocketState.Open)
                {
                    await socket.SendAsync(new ArraySegment<byte>(serverMsg, 0, serverMsg.Length), WebSocketMessageType.Text, true, CancellationToken.None);
                }
            }
        }
    }
}

public class WebSocketHandler
{
    private readonly WebSocketManager _webSocketManager;
    private readonly ILogger<WebSocketHandler> _logger;
    private readonly IConfiguration _configuration;
    private readonly TravelContext _db;

    public WebSocketHandler(WebSocketManager webSocketManager, IConfiguration configuration, ILogger<WebSocketHandler> logger, TravelContext db)
    {
        _webSocketManager = webSocketManager;
        _configuration = configuration;
        _logger = logger;
        _db = db;
    }

    public async Task HandleWebSocketAsync(string channelName, WebSocket webSocket)
    {
        var socketId = await _webSocketManager.AddSocketAsync(channelName, webSocket);

        _logger.LogInformation($"New WebSocket connection in channel {channelName}");

        var cancellationToken = CancellationToken.None;

        while (webSocket.State == WebSocketState.Open)
        {
            await _webSocketManager.BroadcastToChannelAsync(channelName, _configuration["WebSocketMessage:UpdateOrder"]);
            
            await Task.Delay(1000, cancellationToken);
        }

        await _webSocketManager.RemoveSocketAsync(channelName, socketId);
        await webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "WebSocket closed", cancellationToken);
        _logger.Log(LogLevel.Information, $"WebSocket connection closed in channel {channelName}");
    }
}