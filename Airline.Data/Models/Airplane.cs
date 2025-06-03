using System;
using System.Collections.Generic;

namespace Airline.Data.Models;

public class Airplane
{
    public int AirplaneId { get; set; }
    public string TailNumber { get; set; } = null!;
    public string Model { get; set; } = null!;
    public string CapacityClass { get; set; } = null!; // "Small","Medium","Large"
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }

    public ICollection<AirplaneConfiguration> Configurations { get; set; } = new List<AirplaneConfiguration>();
    public AirplaneLocation? Location { get; set; }
    public ICollection<Flight> Flights { get; set; } = new List<Flight>();
}
