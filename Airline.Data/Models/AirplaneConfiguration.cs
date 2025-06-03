using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Airline.Data.Models;

public class AirplaneConfiguration
{
    [Key]
    public int AirplaneConfigId { get; set; }
    public int AirplaneId { get; set; }
    public string ClassName { get; set; } = null!;  // "First","Business","Economy"
    public int SeatCount { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }

    // Navigation
    public Airplane Airplane { get; set; } = null!;
}

