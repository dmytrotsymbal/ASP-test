using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using test.Models; 
using test.Data; 

[Route("api/[controller]")]
[ApiController]
public class EventsController : ControllerBase
{
    private readonly AppDbContext _context;

    public EventsController(AppDbContext context)
    {
        _context = context;
    }

    // GET: api/AccessEvents
    [HttpGet]
    public async Task<ActionResult<IEnumerable<AccessEvent>>> GetAccessEvents()
    {
        return await _context.AccessEvents.ToListAsync();
    }
}
