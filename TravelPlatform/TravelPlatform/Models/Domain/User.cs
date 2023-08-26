using System;
using System.Collections.Generic;

namespace TravelPlatform.Models.Domain;

public partial class User
{
    public long Id { get; set; }

    public string Role { get; set; } = null!;

    public string Provider { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string AccessToken { get; set; } = null!;
}
