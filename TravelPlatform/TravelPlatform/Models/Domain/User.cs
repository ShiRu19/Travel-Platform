using System;
using System.Collections.Generic;

namespace TravelPlatform.Models.Domain;

public partial class User
{
    public long Id { get; set; }

    public string Name { get; set; } = null!;

    public string Sex { get; set; } = null!;

    public DateTime Birthday { get; set; }

    public string Email { get; set; } = null!;

    public string PhoneNumber { get; set; } = null!;

    public string Region { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string Role { get; set; } = null!;
}
