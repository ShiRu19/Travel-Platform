using System;
using System.Collections.Generic;

namespace TravelPlatform.Models.Domain;

public partial class Order
{
    public long Id { get; set; }

    public DateTime OrderDate { get; set; }

    public string Nation { get; set; } = null!;

    public long TravelSessionId { get; set; }

    public long Total { get; set; }

    public long UserId { get; set; }

    public string UserName { get; set; } = null!;

    public string UserEmail { get; set; } = null!;

    public string UserPhoneNumber { get; set; } = null!;

    public int PayStatus { get; set; }

    public string? AccountFiveDigits { get; set; }

    public DateTime? PayDate { get; set; }

    public int CheckStatus { get; set; }

    public DateTime? CheckDate { get; set; }
}
