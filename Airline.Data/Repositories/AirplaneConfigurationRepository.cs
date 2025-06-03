using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Airline.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Airline.Data.Repositories
{
    public class AirplaneConfigurationRepository : IAirplaneConfigurationRepository
    {
        private readonly AirlineDbContext _context;

        public AirplaneConfigurationRepository(AirlineDbContext context)
        {
            _context = context;
        }

        public async Task<AirplaneConfiguration> GetByIdAsync(int id)
        {
            return await _context.AirplaneConfiguration
                .FirstOrDefaultAsync(c => c.AirplaneConfigId == id);
        }

        public async Task<IEnumerable<AirplaneConfiguration>> GetAllAsync()
        {
            return await _context.AirplaneConfiguration.ToListAsync();
        }

        public async Task AddAsync(AirplaneConfiguration config)
        {
            await _context.AirplaneConfiguration.AddAsync(config);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(AirplaneConfiguration config)
        {
            var existing = await _context.AirplaneConfiguration
                .FindAsync(config.AirplaneConfigId);
            if (existing == null)
                throw new InvalidOperationException($"Configuration with Id {config.AirplaneConfigId} not found.");

            existing.ClassName = config.ClassName;
            existing.SeatCount = config.SeatCount;
            existing.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var existing = await _context.AirplaneConfiguration.FindAsync(id);
            if (existing != null)
            {
                _context.AirplaneConfiguration.Remove(existing);
                await _context.SaveChangesAsync();
            }
        }
    }
}
