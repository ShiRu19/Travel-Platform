using System;
using System.Collections.Generic;

namespace TravelPlatform.Models.Domain;

public partial class Chat
{
    public long Id { get; set; }

    public long RoomId { get; set; }

    public int Sender { get; set; }

    public string Message { get; set; } = null!;

    public DateTime SendTime { get; set; }
}
