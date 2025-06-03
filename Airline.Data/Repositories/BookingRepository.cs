using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Airline.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Airline.Data.Repositories
{
    public class BookingRepository : IBookingRepository
    {
        private readonly AirlineDbContext _context;

        public BookingRepository(AirlineDbContext context)
        {
            _context = context;
        }

        public async Task<Booking> GetByIdAsync(int id)
        {
            return await _context.Booking
                .Include(b => b.User)
                .Include(b => b.Flight)
                .FirstOrDefaultAsync(b => b.BookingId == id);
        }

        public async Task<IEnumerable<Booking>> GetAllAsync()
        {
            return await _context.Booking
                .Include(b => b.User)
                .Include(b => b.Flight)
                .ToListAsync();
        }

        public async Task<IEnumerable<Booking>> GetByUserIdAsync(int userId)
        {
            return await _context.Booking
                .Include(b => b.User)
                .Include(b => b.Flight)
                .Where(b => b.UserId == userId)
                .ToListAsync();
        }

        public async Task AddAsync(Booking booking)
        {
            await _context.Booking.AddAsync(booking);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Booking booking)
        {
            var existing = await _context.Booking.FindAsync(booking.BookingId);
            if (existing == null)
                throw new InvalidOperationException($"Booking with Id {booking.BookingId} not found.");

            existing.UserId = booking.UserId;
            existing.FlightId = booking.FlightId;
            existing.SeatNumber = booking.SeatNumber;
            existing.ServiceClass = booking.ServiceClass;
            existing.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var existing = await _context.Booking.FindAsync(id);
            if (existing != null)
            {
                _context.Booking.Remove(existing);
                await _context.SaveChangesAsync();
            }
        }
    }
}
