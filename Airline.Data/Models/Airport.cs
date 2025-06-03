using System;
using System.Collections.Generic;

namespace Airline.Data.Models;

public class Airport
{
    public int AirportId { get; set; }
    public string Code { get; set; } = null!;
    public string Name { get; set; } = null!;
    public string City { get; set; } = null!;
    public string Country { get; set; } = null!;
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }

    public ICollection<Flight> DepartingFlights { get; set; } = new List<Flight>();
    public ICollection<Flight> ArrivingFlights { get; set; } = new List<Flight>();
    public ICollection<AirplaneLocation> AirplaneLocations { get; set; } = new List<AirplaneLocation>();
}
