using System;
using System.Collections.Generic;

namespace TravelPlatform.Models.Domain;

public partial class Order
{
    public long Id { get; set; }

    public DateTime OrderDate { get; set; }

    public string Nation { get; set; } = null!;

    public long TravelSessionId { get; set; }

    public long UserId { get; set; }

    public long Total { get; set; }

    public int Check { get; set; }

    public DateTime? CheckDate { get; set; }
}
