using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Airline.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Airline.Data.Repositories
{
    public class AirplaneRepository : IAirplaneRepository
    {
        private readonly AirlineDbContext _context;

        public AirplaneRepository(AirlineDbContext context)
        {
            _context = context;
        }

        public async Task<Airplane> GetByIdAsync(int id)
        {
            return await _context.Airplane
                .Include(a => a.Configurations)
                .Include(a => a.Location)
                .FirstOrDefaultAsync(a => a.AirplaneId == id);
        }

        public async Task<IEnumerable<Airplane>> GetAllAsync()
        {
            return await _context.Airplane
                .Include(a => a.Configurations)
                .Include(a => a.Location)
                .ToListAsync();
        }

        public async Task AddAsync(Airplane airplane)
        {
            await _context.Airplane.AddAsync(airplane);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Airplane airplane)
        {
            var existing = await _context.Airplane.FindAsync(airplane.AirplaneId);
            if (existing == null)
                throw new InvalidOperationException($"Airplane with Id {airplane.AirplaneId} not found.");

            existing.TailNumber = airplane.TailNumber;
            existing.Model = airplane.Model;
            existing.CapacityClass = airplane.CapacityClass;
            existing.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var existing = await _context.Airplane.FindAsync(id);
            if (existing != null)
            {
                _context.Airplane.Remove(existing);
                await _context.SaveChangesAsync();
            }
        }
    }
}
