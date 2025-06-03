using System;
using System.Collections.Generic;

namespace Airline.Data.Models;

public class Flight
{
    public int FlightId { get; set; }
    public string FlightNumber { get; set; } = null!;
    public DateTime DepartureTime { get; set; }
    public DateTime ArrivalTime { get; set; }
    public int OriginAirportId { get; set; }
    public int DestinationAirportId { get; set; }
    public int AirplaneId { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }

    // Navigation:
    public Airport OriginAirport { get; set; } = null!;
    public Airport DestinationAirport { get; set; } = null!;
    public Airplane Airplane { get; set; } = null!;
    public ICollection<Booking> Bookings { get; set; } = new List<Booking>();
}
