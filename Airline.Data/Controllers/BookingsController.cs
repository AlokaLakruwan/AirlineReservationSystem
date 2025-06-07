using Airline.Data.Repositories;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/v1/[controller]")]
public class BookingsController : ControllerBase
{
    private readonly IBookingRepository _repo;
    public BookingsController(IBookingRepository repo) => _repo = repo;

    [HttpPost]
    public async Task<ActionResult<Booking>> Create(Booking newBooking)
    {
        await _repo.AddAsync(newBooking);
        return CreatedAtAction(nameof(GetById),
            new { id = newBooking.BookingId },
            newBooking);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, Booking update)
    {
        if (id != update.BookingId)
            return BadRequest("Route ID must match BookingId.");

        await _repo.UpdateAsync(update);
        return NoContent();
    }

    // GET /api/v1/Bookings
    [HttpGet]
    public Task<IEnumerable<Booking>> GetAll() => _repo.GetAllAsync();

    // GET /api/v1/Bookings/{id}
    [HttpGet("{id}")]
    public async Task<ActionResult<Booking>> GetById(int id)
    {
        var b = await _repo.GetByIdAsync(id);
        return b == null ? NotFound() : b;
    }

    // DELETE /api/v1/Bookings/{id}
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        await _repo.DeleteAsync(id);
        return NoContent();
    }
}
