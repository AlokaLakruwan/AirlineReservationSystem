using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Airline.Data.Models;

public class AirplaneLocation
{
    [Key]
    public int AirplaneId { get; set; }
    public int CurrentAirportId { get; set; }
    public DateTime LastUpdated { get; set; }

    public Airplane Airplane { get; set; } = null!;
    public Airport Airport { get; set; } = null!;
}
