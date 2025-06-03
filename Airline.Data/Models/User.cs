using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Airline.Data.Models;

public class User
{
    public int UserId { get; set; }
    public string Username { get; set; } = null!;
    public string PasswordHash { get; set; } = null!;
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string Email { get; set; } = null!;
    public int RoleId { get; set; }
    public bool IsActive { get; set; } = true;
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }

    // Navigation
    //[JsonIgnore]
    public Role Role { get; set; } = null!;
    public ICollection<Booking> Bookings { get; set; } = new List<Booking>();
}
