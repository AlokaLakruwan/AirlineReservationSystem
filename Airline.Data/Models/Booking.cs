using System;
using System.Collections.Generic;

namespace Airline.Data.Models;

public class Booking
{
    public int BookingId { get; set; }
    public DateTime BookingDate { get; set; }
    public int UserId { get; set; }
    public int FlightId { get; set; }
    public string SeatNumber { get; set; } = null!;
    public string ServiceClass { get; set; } = null!; // "First","Business","Economy"
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }

    // Navigation
    public User User { get; set; } = null!;
    public Flight Flight { get; set; } = null!;
}
