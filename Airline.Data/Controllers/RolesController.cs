using Airline.Data.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[ApiController]
[Route("api/v1/[controller]")]
public class RolesController : ControllerBase
{
    private readonly AirlineDbContext _ctx;
    public RolesController(AirlineDbContext ctx) => _ctx = ctx;

    // GET api/v1/Roles
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Role>>> GetAll()
    {
        return await _ctx.Role.ToListAsync();
    }
}