using System;
using System.Collections.Generic;

namespace TravelPlatform.Models.Domain;

public partial class TravelAttraction
{
    public long Id { get; set; }

    public long TravelId { get; set; }

    public string Attraction { get; set; } = null!;

    public virtual Travel Travel { get; set; } = null!;
}
