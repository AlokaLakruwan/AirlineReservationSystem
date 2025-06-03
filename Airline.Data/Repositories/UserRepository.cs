using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Airline.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Airline.Data.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly AirlineDbContext _context;

        public UserRepository(AirlineDbContext context)
        {
            _context = context;
        }

        // GET User by primary key: UserId
        public async Task<User> GetByIdAsync(int id)
        {
            return await _context.User
                .Include(u => u.Role)
                .FirstOrDefaultAsync(u => u.UserId == id);
        }

        // GET all User
        public async Task<IEnumerable<User>> GetAllAsync()
        {
            return await _context.User
                .Include(u => u.Role)
                .ToListAsync();
        }

        // CREATE a new User
        public async Task AddAsync(User user)
        {
            await _context.User.AddAsync(user);
            await _context.SaveChangesAsync();
        }

        // UPDATE an existing User (expects user.UserId to be set)
        public async Task UpdateAsync(User user)
        {
            // Optional: ensure it exists first
            var existing = await _context.User.FindAsync(user.UserId);
            if (existing == null)
                throw new InvalidOperationException($"User with Id {user.UserId} not found.");

            // Copy properties (example; adapt as needed)
            existing.Username = user.Username;
            existing.PasswordHash = user.PasswordHash;
            existing.FirstName = user.FirstName;
            existing.LastName = user.LastName;
            existing.Email = user.Email;
            existing.RoleId = user.RoleId;   // previously RoleID
            existing.IsActive = user.IsActive;
            existing.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
        }

        // DELETE a User by UserId
        public async Task DeleteAsync(int id)
        {
            var existing = await _context.User.FindAsync(id);
            if (existing != null)
            {
                _context.User.Remove(existing);
                await _context.SaveChangesAsync();
            }
        }

        // GET user by username
        public async Task<User> GetByUsernameAsync(string username)
        {
            return await _context.User
                .Include(u => u.Role)
                .FirstOrDefaultAsync(u => u.Username == username);
        }
    }
}
