using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Airline.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Airline.Data.Repositories
{
    public class AirplaneLocationRepository : IAirplaneLocationRepository
    {
        private readonly AirlineDbContext _context;

        public AirplaneLocationRepository(AirlineDbContext context)
        {
            _context = context;
        }

        public async Task<AirplaneLocation> GetByAirplaneIdAsync(int airplaneId)
        {
            return await _context.AirplaneLocation
                .Include(l => l.Airplane)
                .Include(l => l.Airport)
                .FirstOrDefaultAsync(l => l.AirplaneId == airplaneId);
        }

        public async Task<IEnumerable<AirplaneLocation>> GetAllAsync()
        {
            return await _context.AirplaneLocation
                .Include(l => l.Airplane)
                .Include(l => l.Airport)
                .ToListAsync();
        }

        public async Task AddAsync(AirplaneLocation location)
        {
            await _context.AirplaneLocation.AddAsync(location);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(AirplaneLocation location)
        {
            var existing = await _context.AirplaneLocation
                .FindAsync(location.AirplaneId);
            if (existing == null)
                throw new InvalidOperationException($"Location for airplane {location.AirplaneId} not found.");

            existing.CurrentAirportId = location.CurrentAirportId;
            existing.LastUpdated = DateTime.UtcNow;

            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int airplaneId)
        {
            var existing = await _context.AirplaneLocation.FindAsync(airplaneId);
            if (existing != null)
            {
                _context.AirplaneLocation.Remove(existing);
                await _context.SaveChangesAsync();
            }
        }
    }
}
