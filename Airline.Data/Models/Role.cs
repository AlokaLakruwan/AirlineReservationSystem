using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Airline.Data.Models;

public class Role
{
    public int RoleId { get; set; }
    public string RoleName { get; set; } = null!;
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }

    // Navigation
    //[JsonIgnore]
    public ICollection<User> Users { get; set; } = new List<User>();
}
