using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc.ModelBinding;            // for BindNever
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation; // for ValidateNever

namespace Airline.Data.Models
{
    public class Flight
    {
        public int FlightId { get; private set; }

        public string FlightNumber { get; set; } = null!;

        [Column(TypeName = "datetimeoffset")]
        public DateTimeOffset DepartureTime { get; set; }

        [Column(TypeName = "datetimeoffset")]
        public DateTimeOffset ArrivalTime { get; set; }
        public int OriginAirportId { get; set; }
        public int DestinationAirportId { get; set; }
        public int AirplaneId { get; set; }

        [Column(TypeName = "datetimeoffset")]
        public DateTimeOffset CreatedAt { get; internal set; }

        [Column(TypeName = "datetimeoffset")]
        public DateTimeOffset? UpdatedAt { get; internal set; }

        [JsonIgnore]
        [BindNever]
        [ValidateNever]
        public Airport OriginAirport { get; internal set; } = null!;

        [JsonIgnore]
        [BindNever]
        [ValidateNever]
        public Airport DestinationAirport { get; internal set; } = null!;

        [JsonIgnore]
        [BindNever]
        [ValidateNever]
        public Airplane Airplane { get; internal set; } = null!;

        [JsonIgnore]
        [BindNever]
        [ValidateNever]
        public ICollection<Booking> Bookings { get; internal set; } = new List<Booking>();
    }
}
