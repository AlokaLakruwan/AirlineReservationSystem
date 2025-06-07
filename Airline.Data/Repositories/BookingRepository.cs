using Airline.Data.Repositories;
using Microsoft.EntityFrameworkCore;

public class BookingRepository : IBookingRepository
{
    private readonly AirlineDbContext _context;
    public BookingRepository(AirlineDbContext ctx) => _context = ctx;

    public async Task<Booking> GetByIdAsync(int id) =>
        await _context.Booking
            .Include(b => b.User)
            .Include(b => b.Flight)
            .FirstOrDefaultAsync(b => b.BookingId == id);

    public async Task<IEnumerable<Booking>> GetAllAsync() =>
        await _context.Booking
            .Include(b => b.User)
            .Include(b => b.Flight)
            .ToListAsync();

    public async Task<IEnumerable<Booking>> GetByUserIdAsync(int userId) =>
        await _context.Booking
            .Where(b => b.UserId == userId)
            .Include(b => b.Flight)
            .ToListAsync();

    public async Task AddAsync(Booking newBooking)
    {
        newBooking.CreatedAt = DateTime.UtcNow;
        await _context.Booking.AddAsync(newBooking);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Booking booking)
    {
        var existing = await _context.Booking.FindAsync(booking.BookingId);
        if (existing == null) throw new InvalidOperationException($"Booking {booking.BookingId} not found.");

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
