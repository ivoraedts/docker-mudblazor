using Microsoft.AspNetCore.Mvc;
using MyApplication.Data;

[Route("api/[controller]")]
[ApiController]
public class CalendarEventController : ControllerBase
{
    private readonly MudBlazorDbContext _context;
    public CalendarEventController(MudBlazorDbContext context)
    {
        _context = context;
    }

    // GET: api/Values
    [HttpGet]
    public async Task<ActionResult<IEnumerable<CalendarEvent>>> CalendarEvents()
    {
        return await _context.CalendarEvents.AsAsyncEnumerable().OrderBy(e => e.Id).ToListAsync();
    }

    // GET: api/Values/5
    [HttpGet("{id}")]
    public async Task<ActionResult<CalendarEvent>> GetCalendarEvent(int id)
    {
        var calendarEvent = await _context.CalendarEvents.FindAsync(id);
        if (calendarEvent == null)
        {
            return NotFound();
        }
        return Ok(calendarEvent);
    }

    // POST: api/Values
    [HttpPost]
    public async Task<ActionResult<CalendarEvent>> PostCalendarEvent(CalendarEvent calendarEvent)
    {
        _context.CalendarEvents.Add(calendarEvent);
        await _context.SaveChangesAsync();
        return Ok(calendarEvent);
    }

    // PUT: api/Values/5
    [HttpPut("{id}")]
    public async Task<IActionResult> PutCalendarEvent(int id, CalendarEvent calendarEvent)
    {
        if (id != calendarEvent.Id)
        {
            return BadRequest();
        }

        _context.Entry(calendarEvent).State = Microsoft.EntityFrameworkCore.EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
            return Ok(calendarEvent);
        }
        catch (Microsoft.EntityFrameworkCore.DbUpdateConcurrencyException)
        {
            if (!_context.CalendarEvents.Any(e => e.Id == id))
            {
                return NotFound();
            }
            else
            {
                throw;
            }
        }
    }

    // DELETE: api/Values/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteCalendarEvent(int id)
    {
        var calendarEvent = await _context.CalendarEvents.FindAsync(id);
        if (calendarEvent == null)
        {
            return NotFound();
        }

        _context.CalendarEvents.Remove(calendarEvent);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}