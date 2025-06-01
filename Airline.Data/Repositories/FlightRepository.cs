using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Airline.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Airline.Data.Repositories
{
    public class FlightRepository : IFlightRepository
    {
        private readonly AirlineDbContext _context;

        public FlightRepository(AirlineDbContext context)
        {
            _context = context;
        }

        public async Task<Flight> GetByIdAsync(int id)
        {
            return await _context.Flights
                .Include(f => f.Airplane)
                .Include(f => f.OriginAirport)
                .Include(f => f.DestinationAirport)
                .FirstOrDefaultAsync(f => f.FlightId == id);
        }

        public async Task<IEnumerable<Flight>> GetAllAsync()
        {
            return await _context.Flights
                .Include(f => f.Airplane)
                .Include(f => f.OriginAirport)
                .Include(f => f.DestinationAirport)
                .ToListAsync();
        }

        public async Task<IEnumerable<Flight>> SearchAsync(string originCode, string destinationCode, DateTime date)
        {
            return await _context.Flights
                .Include(f => f.Airplane)
                .Include(f => f.OriginAirport)
                .Include(f => f.DestinationAirport)
                .Where(f =>
                    f.OriginAirport.Code == originCode &&
                    f.DestinationAirport.Code == destinationCode &&
                    f.DepartureTime.HasValue &&
                    f.DepartureTime.Value.Date == date.Date
                )
                .ToListAsync();
        }

        public async Task AddAsync(Flight flight)
        {
            flight.CreatedAt = DateTime.UtcNow;
            flight.UpdatedAt = DateTime.UtcNow;
            await _context.Flights.AddAsync(flight);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Flight flight)
        {
            flight.UpdatedAt = DateTime.UtcNow;
            _context.Flights.Update(flight);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var flight = await _context.Flights.FindAsync(id);
            if (flight != null)
            {
                _context.Flights.Remove(flight);
                await _context.SaveChangesAsync();
            }
        }
    }
}
