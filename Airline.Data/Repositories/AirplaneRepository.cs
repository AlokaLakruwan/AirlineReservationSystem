using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
            return await _context.Airplanes.FindAsync(id);
        }

        public async Task<IEnumerable<Airplane>> GetAllAsync()
        {
            return await _context.Airplanes.ToListAsync();
        }

        public async Task AddAsync(Airplane airplane)
        {
            airplane.CreatedAt = DateTime.UtcNow;
            airplane.UpdatedAt = DateTime.UtcNow;
            await _context.Airplanes.AddAsync(airplane);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Airplane airplane)
        {
            airplane.UpdatedAt = DateTime.UtcNow;
            _context.Airplanes.Update(airplane);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var airplane = await _context.Airplanes.FindAsync(id);
            if (airplane != null)
            {
                _context.Airplanes.Remove(airplane);
                await _context.SaveChangesAsync();
            }
        }
    }
}
