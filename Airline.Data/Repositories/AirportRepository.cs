using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
            return await _context.Airports.FindAsync(id);
        }

        public async Task<IEnumerable<Airport>> GetAllAsync()
        {
            return await _context.Airports.ToListAsync();
        }

        public async Task AddAsync(Airport airport)
        {
            airport.CreatedAt = DateTime.UtcNow;
            airport.UpdatedAt = DateTime.UtcNow;
            await _context.Airports.AddAsync(airport);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Airport airport)
        {
            airport.UpdatedAt = DateTime.UtcNow;
            _context.Airports.Update(airport);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var airport = await _context.Airports.FindAsync(id);
            if (airport != null)
            {
                _context.Airports.Remove(airport);
                await _context.SaveChangesAsync();
            }
        }
    }
}
