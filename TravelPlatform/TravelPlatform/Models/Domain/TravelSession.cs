using System;
using System.Collections.Generic;

namespace TravelPlatform.Models.Domain;

public partial class TravelSession
{
    public long Id { get; set; }

    public long TravelId { get; set; }

    public string ProductNumber { get; set; } = null!;

    public DateTime DepartureDate { get; set; }

    public int Price { get; set; }

    public int RemainingSeats { get; set; }

    public int Seats { get; set; }

    public int GroupStatus { get; set; }
}
