using System;
using System.Collections.Generic;

namespace Airline.Data.Models;

public partial class Flight
{
    public int FlightId { get; set; }

    public string? FlightNumber { get; set; }

    public DateTime? DepartureTime { get; set; }

    public DateTime? ArrivalTime { get; set; }

    public int? OriginAirportId { get; set; }

    public int? DestinationAirportId { get; set; }

    public int? AirplaneId { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual Airplane? Airplane { get; set; }

    public virtual ICollection<Booking> Bookings { get; set; } = new List<Booking>();

    public virtual Airport? DestinationAirport { get; set; }

    public virtual Airport? OriginAirport { get; set; }
}
