using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Airline.Data.Models;
using Airline.Data.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Airline.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FlightsController : ControllerBase
    {
        private readonly IFlightRepository _flightRepo;

        public FlightsController(IFlightRepository flightRepo)
        {
            _flightRepo = flightRepo;
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
            return flight;
        }

        // GET: api/flights/search?origin=CMB&destination=DEL&date=2025-06-02
        [HttpGet("search")]
        public async Task<IEnumerable<Flight>> SearchFlights(
            [FromQuery] string origin,
            [FromQuery] string destination,
            [FromQuery] DateTime date)
        {
            return await _flightRepo.SearchAsync(origin, destination, date);
        }

        [HttpPost]
        public async Task<ActionResult<Flight>> Create(Flight newFlight)
        {
            await _flightRepo.AddAsync(newFlight);
            return CreatedAtAction(nameof(GetById), new { id = newFlight.FlightId }, newFlight);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, Flight updatedFlight)
        {
            if (id != updatedFlight.FlightId) return BadRequest();
            await _flightRepo.UpdateAsync(updatedFlight);
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

