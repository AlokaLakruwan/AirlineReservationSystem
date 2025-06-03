using System;
using System.Collections.Generic;
using System.Linq;
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
            return await _context.Flight
                .Include(f => f.OriginAirport)
                .Include(f => f.DestinationAirport)
                .Include(f => f.Airplane)
                .FirstOrDefaultAsync(f => f.FlightId == id);
        }

        public async Task<IEnumerable<Flight>> GetAllAsync()
        {
            return await _context.Flight
                .Include(f => f.OriginAirport)
                .Include(f => f.DestinationAirport)
                .Include(f => f.Airplane)
                .ToListAsync();
        }

        public async Task<IEnumerable<Flight>> SearchAsync(int originAirportId, int destinationAirportId, DateTime date)
        {
            return await _context.Flight
                .Include(f => f.OriginAirport)
                .Include(f => f.DestinationAirport)
                .Include(f => f.Airplane)
                .Where(f =>
                    f.OriginAirportId == originAirportId &&
                    f.DestinationAirportId == destinationAirportId &&
                    f.DepartureTime.Date == date.Date)
                .ToListAsync();
        }

        public async Task AddAsync(Flight flight)
        {
            await _context.Flight.AddAsync(flight);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Flight flight)
        {
            var existing = await _context.Flight.FindAsync(flight.FlightId);
            if (existing == null)
                throw new InvalidOperationException($"Flight with Id {flight.FlightId} not found.");

            existing.FlightNumber = flight.FlightNumber;
            existing.DepartureTime = flight.DepartureTime;
            existing.ArrivalTime = flight.ArrivalTime;
            existing.OriginAirportId = flight.OriginAirportId;
            existing.DestinationAirportId = flight.DestinationAirportId;
            existing.AirplaneId = flight.AirplaneId;
            existing.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var existing = await _context.Flight.FindAsync(id);
            if (existing != null)
            {
                _context.Flight.Remove(existing);
                await _context.SaveChangesAsync();
            }
        }
    }
}
