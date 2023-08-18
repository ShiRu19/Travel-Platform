using System;
using System.Collections.Generic;

namespace TravelPlatform.Models.Domain;

public partial class Follow
{
    public long Id { get; set; }

    public long TravelId { get; set; }

    public long MemberId { get; set; }
}
