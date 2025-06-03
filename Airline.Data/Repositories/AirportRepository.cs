using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Airline.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Airline.Data.Repositories
{
    public class AirportRepository : IAirportRepository
    {
        private readonly AirlineDbContext _context;

        public AirportRepository(AirlineDbContext context)
        {
            _context = context;
        }

        public async Task<Airport> GetByIdAsync(int id)
        {
            return await _context.Airport
                .FirstOrDefaultAsync(a => a.AirportId == id);
        }

        public async Task<Airport> GetByCodeAsync(string code)
        {
            return await _context.Airport.FirstOrDefaultAsync(a => a.Code == code);
        }

        public async Task<IEnumerable<Airport>> GetAllAsync()
        {
            return await _context.Airport.ToListAsync();
        }

        public async Task AddAsync(Airport airport)
        {
            await _context.Airport.AddAsync(airport);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Airport airport)
        {
            var existing = await _context.Airport.FindAsync(airport.AirportId);
            if (existing == null)
                throw new InvalidOperationException($"Airport with Id {airport.AirportId} not found.");

            existing.Code = airport.Code;
            existing.Name = airport.Name;
            existing.City = airport.City;
            existing.Country = airport.Country;
            existing.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var existing = await _context.Airport.FindAsync(id);
            if (existing != null)
            {
                _context.Airport.Remove(existing);
                await _context.SaveChangesAsync();
            }
        }
    }
}
