using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Airline.Data.Models;
using Airline.Data.Repositories;
using System.Runtime.InteropServices;
using Microsoft.AspNetCore.Mvc;
using Airline.Data.Helpers;

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
            if (!string.IsNullOrWhiteSpace(newUser.Password))
            {
                newUser.PasswordHash = HashHelper.ComputeSha256Hash(newUser.Password);
            }
            newUser.Password = null;

            await _userRepo.AddAsync(newUser);
            return CreatedAtAction(nameof(GetById), new { id = newUser.UserId }, newUser);
        }

        // PUT: api/users/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, User user)
        {
            // 1) Load the tracked entity
            var existing = await _userRepo.GetByIdAsync(id);
            if (existing == null)
                return NotFound();

            // 2) Copy over only the updatable fields
            existing.Username = user.Username;
            existing.FirstName = user.FirstName;
            existing.LastName = user.LastName;
            existing.Email = user.Email;
            existing.RoleId = user.RoleId;
            existing.IsActive = user.IsActive;
            existing.UpdatedAt = DateTime.UtcNow;

            // 3) If the client supplied a new plain-text password, hash & store it
            if (!string.IsNullOrWhiteSpace(user.Password))
            {
                existing.PasswordHash = HashHelper.ComputeSha256Hash(user.Password);
            }
            // make sure we don’t persist the transient Password field
            user.Password = null;

            // 4) Persist _the existing_ entity—NOT the incoming `user`
            await _userRepo.UpdateAsync(existing);

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
