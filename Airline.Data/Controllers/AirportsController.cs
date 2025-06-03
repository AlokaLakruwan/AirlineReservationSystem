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
    [Route("api/v1/[controller]")]
    [ApiController]
    public class AirportsController : ControllerBase
    {
        private readonly IAirportRepository _airportRepo;

        public AirportsController(IAirportRepository airportRepo)
        {
            _airportRepo = airportRepo;
        }

        [HttpGet]
        public async Task<IEnumerable<Airport>> GetAll()
        {
            return await _airportRepo.GetAllAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Airport>> GetById(int id)
        {
            var airport = await _airportRepo.GetByIdAsync(id);
            if (airport == null) return NotFound();
            return airport;
        }

        [HttpPost]
        public async Task<ActionResult<Airport>> Create(Airport newAirport)
        {
            await _airportRepo.AddAsync(newAirport);
            return CreatedAtAction(nameof(GetById), new { id = newAirport.AirportId }, newAirport);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, Airport updatedAirport)
        {
            if (id != updatedAirport.AirportId) return BadRequest();
            await _airportRepo.UpdateAsync(updatedAirport);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _airportRepo.DeleteAsync(id);
            return NoContent();
        }
    }
}
