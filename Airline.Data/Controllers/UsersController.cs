using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Airline.Data.Models;
using Airline.Data.Repositories;
using System.Runtime.InteropServices;
using Microsoft.AspNetCore.Mvc;

namespace Airline.Api.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserRepository _userRepo;

        public UsersController(IUserRepository userRepo)
        {
            _userRepo = userRepo;
        }

        // GET: api/users
        [HttpGet]
        public async Task<IEnumerable<User>> GetAll()
        {
            return await _userRepo.GetAllAsync();
        }

        // GET: api/users/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetById(int id)
        {
            var user = await _userRepo.GetByIdAsync(id);
            if (user == null) return NotFound();
            return user;
        }

        // GET: api/users/username/{username}
        [HttpGet("username/{username}")]
        public async Task<ActionResult<User>> GetByUsername(string username)
        {
            var user = await _userRepo.GetByUsernameAsync(username);
            if (user == null) return NotFound();
            return user;
        }

        // POST: api/users
        [HttpPost]
        public async Task<ActionResult<User>> Create(User newUser)
        {
            await _userRepo.AddAsync(newUser);
            return CreatedAtAction(nameof(GetById), new { id = newUser.UserId }, newUser);
        }

        // PUT: api/users/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, User updatedUser)
        {
            if (id != updatedUser.UserId) return BadRequest();
            await _userRepo.UpdateAsync(updatedUser);
            return NoContent();
        }

        // DELETE: api/users/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _userRepo.DeleteAsync(id);
            return NoContent();
        }
    }
}
