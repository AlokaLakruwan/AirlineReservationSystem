using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Airline.Data.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

public class Booking
{
    public int BookingId { get; private set; }

    public int UserId { get; set; }
    public int FlightId { get; set; }
    public string SeatNumber { get; set; } = null!;
    public string ServiceClass { get; set; } = null!;

    [Column(TypeName = "datetimeoffset")]
    public DateTimeOffset BookingDate { get; internal set; }

    [Column(TypeName = "datetimeoffset")]
    public DateTimeOffset CreatedAt { get; internal set; }

    [Column(TypeName = "datetimeoffset")]
    public DateTimeOffset? UpdatedAt { get; internal set; }

 

    [JsonIgnore]
    [BindNever]
    [ValidateNever]
    public User User { get; internal set; } = null!;

    [JsonIgnore]
    [BindNever]
    [ValidateNever]
    public Flight Flight { get; internal set; } = null!;
}
