using System;
using System.Collections.Generic;

namespace Airline.Data.Models;

public partial class Airplane
{
    public int AirplaneId { get; set; }

    public string? TailNumber { get; set; }

    public string? Model { get; set; }

    public string? CapacityClass { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual ICollection<Flight> Flights { get; set; } = new List<Flight>();
}
