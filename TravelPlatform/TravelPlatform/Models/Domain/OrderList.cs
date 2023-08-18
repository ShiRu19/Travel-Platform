using System;
using System.Collections.Generic;

namespace TravelPlatform.Models.Domain;

public partial class OrderList
{
    public long Id { get; set; }

    public long OrderId { get; set; }

    public int Price { get; set; }

    public string Name { get; set; } = null!;

    public string Sex { get; set; } = null!;

    public DateTime Birthday { get; set; }

    public string PhoneNumber { get; set; } = null!;

    public string? LastName { get; set; }

    public string? FirstName { get; set; }

    public string? IdentityCode { get; set; }

    public string? PassportNumber { get; set; }

    public string? SpecialNeed { get; set; }
}
