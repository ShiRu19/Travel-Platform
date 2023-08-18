using System;
using System.Collections.Generic;

namespace TravelPlatform.Models.Domain;

public partial class Travel
{
    public long Id { get; set; }

    public string Title { get; set; } = null!;

    public DateTime DateRangeStart { get; set; }

    public DateTime DateRangeEnd { get; set; }

    public int Days { get; set; }

    public string DepartureLocation { get; set; } = null!;

    public string PdfUrl { get; set; } = null!;

    public string MainImageUrl { get; set; } = null!;
}
