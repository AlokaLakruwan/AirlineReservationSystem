using System;
using System.Collections.Generic;

namespace Airline.Data.Models;

public partial class Airport
{
    public int AirportId { get; set; }

    public string? Code { get; set; }

    public string? Name { get; set; }

    public string? City { get; set; }

    public string? Country { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual ICollection<Flight> FlightDestinationAirports { get; set; } = new List<Flight>();

    public virtual ICollection<Flight> FlightOriginAirports { get; set; } = new List<Flight>();
}
