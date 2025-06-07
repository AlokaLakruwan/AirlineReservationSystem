using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Airline.Data.Models;
using Airline.Data.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Airline.Api.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class FlightsController : ControllerBase
    {
        private readonly IFlightRepository _flightRepo;
        private readonly IAirportRepository _airportRepo;

        public FlightsController(
            IFlightRepository flightRepo,
            IAirportRepository airportRepo)
        {
            _flightRepo = flightRepo;
            _airportRepo = airportRepo;
        }

        [HttpGet]
        public async Task<IEnumerable<Flight>> GetAll()
        {
            return await _flightRepo.GetAllAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Flight>> GetById(int id)
        {
            var flight = await _flightRepo.GetByIdAsync(id);
            if (flight == null) return NotFound();
            return Ok(flight);
        }

        // GET: api/v1/flights/search?origin=CMB&destination=DEL&date=2025-06-02
        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<Flight>>> SearchFlights(
            [FromQuery] string origin,
            [FromQuery] string destination,
            [FromQuery] DateTime date)
        {
            // 1. Look up origin airport by code
            var originAirport = await _airportRepo.GetByCodeAsync(origin);
            if (originAirport == null)
                return BadRequest($"Origin airport '{origin}' not found.");

            // 2. Look up destination airport by code
            var destinationAirport = await _airportRepo.GetByCodeAsync(destination);
            if (destinationAirport == null)
                return BadRequest($"Destination airport '{destination}' not found.");

            // 3. Call repository using their integer IDs
            var flights = await _flightRepo.SearchAsync(
                originAirport.AirportId,
                destinationAirport.AirportId,
                date
            );

            return Ok(flights);
        }

        [HttpPost]
        public async Task<ActionResult<Flight>> Create([FromBody] Flight newFlight)
        {
            await _flightRepo.AddAsync(newFlight);
            return CreatedAtAction(nameof(GetById),
                                   new { id = newFlight.FlightId },
                                   newFlight);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Flight input)
        {
            var existing = await _flightRepo.GetByIdAsync(id);
            if (existing == null)
                return NotFound();

            existing.FlightNumber = input.FlightNumber;
            existing.DepartureTime = input.DepartureTime;
            existing.ArrivalTime = input.ArrivalTime;
            existing.OriginAirportId = input.OriginAirportId;
            existing.DestinationAirportId = input.DestinationAirportId;
            existing.AirplaneId = input.AirplaneId;
            existing.UpdatedAt = DateTime.UtcNow;
            await _flightRepo.UpdateAsync(existing);

            return NoContent();
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _flightRepo.DeleteAsync(id);
            return NoContent();
        }
    }
}
