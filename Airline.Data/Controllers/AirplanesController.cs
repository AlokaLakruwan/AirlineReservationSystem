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
    public class AirplanesController : ControllerBase
    {
        private readonly IAirplaneRepository _airplaneRepo;

        public AirplanesController(IAirplaneRepository airplaneRepo)
        {
            _airplaneRepo = airplaneRepo;
        }

        [HttpGet]
        public async Task<IEnumerable<Airplane>> GetAll()
        {
            return await _airplaneRepo.GetAllAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Airplane>> GetById(int id)
        {
            var airplane = await _airplaneRepo.GetByIdAsync(id);
            if (airplane == null) return NotFound();
            return airplane;
        }

        [HttpPost]
        public async Task<ActionResult<Airplane>> Create(Airplane newAirplane)
        {
            await _airplaneRepo.AddAsync(newAirplane);
            return CreatedAtAction(nameof(GetById), new { id = newAirplane.AirplaneId }, newAirplane);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, Airplane updatedAirplane)
        {
            if (id != updatedAirplane.AirplaneId) return BadRequest();
            await _airplaneRepo.UpdateAsync(updatedAirplane);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _airplaneRepo.DeleteAsync(id);
            return NoContent();
        }
    }
}

