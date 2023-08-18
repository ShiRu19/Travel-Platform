using System;
using System.Collections.Generic;

namespace TravelPlatform.Models.Domain;

public partial class Watch
{
    public long Id { get; set; }

    public DateTime Date { get; set; }

    public long TravelId { get; set; }

    public long MemberId { get; set; }

    public TimeSpan? StayTime { get; set; }
}
