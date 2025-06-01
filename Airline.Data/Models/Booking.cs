using System;
using System.Collections.Generic;

namespace Airline.Data.Models;

public partial class Booking
{
    public int BookingId { get; set; }

    public DateTime? BookingDate { get; set; }

    public int? UserId { get; set; }

    public int? FlightId { get; set; }

    public string? SeatNumber { get; set; }

    public string? ServiceClass { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual Flight? Flight { get; set; }

    public virtual User? User { get; set; }
}
