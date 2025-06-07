using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace Airline.Data.Models;

public class User
{
    public int UserId { get; private set; }
    public string Username { get; set; } = null!;

    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string Email { get; set; } = null!;
    public int RoleId { get; set; }
    public bool IsActive { get; set; } = true;
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }

    [NotMapped]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public string? Password { get; set; }
    [JsonIgnore]
    [BindNever]
    [ValidateNever]
    public string? PasswordHash { get; set; } = null!;

    // Navigation
    [JsonIgnore]
    [BindNever]
    [ValidateNever]
    public Role? Role { get; set; } = null!;
    public ICollection<Booking> Bookings { get; set; } = new List<Booking>();
}
