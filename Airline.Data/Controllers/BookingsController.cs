using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Airline.Data.Models;
using Airline.Data.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Airline.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookingsController : ControllerBase
    {
        private readonly IBookingRepository _bookingRepo;

        public BookingsController(IBookingRepository bookingRepo)
        {
            _bookingRepo = bookingRepo;
        }

        [HttpGet]
        public async Task<IEnumerable<Booking>> GetAll()
        {
            return await _bookingRepo.GetAllAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Booking>> GetById(int id)
        {
            var booking = await _bookingRepo.GetByIdAsync(id);
            if (booking == null) return NotFound();
            return booking;
        }

        // GET: api/bookings/user/3
        [HttpGet("user/{userId}")]
        public async Task<IEnumerable<Booking>> GetByUserId(int userId)
        {
            return await _bookingRepo.GetByUserIdAsync(userId);
        }

        [HttpPost]
        public async Task<ActionResult<Booking>> Create(Booking newBooking)
        {
            await _bookingRepo.AddAsync(newBooking);
            return CreatedAtAction(nameof(GetById), new { id = newBooking.BookingId }, newBooking);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, Booking updatedBooking)
        {
            if (id != updatedBooking.BookingId) return BadRequest();
            await _bookingRepo.UpdateAsync(updatedBooking);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _bookingRepo.DeleteAsync(id);
            return NoContent();
        }
    }
}
