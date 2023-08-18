using System;
using System.Collections.Generic;

namespace TravelPlatform.Models.Domain;

public partial class Order
{
    public long Id { get; set; }

    public long TravelSessionId { get; set; }

    public long UserId { get; set; }

    public long Total { get; set; }
}
