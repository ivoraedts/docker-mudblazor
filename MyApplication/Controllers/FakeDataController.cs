using Microsoft.AspNetCore.Mvc;
using MyApplication.Data;

[Route("api/[controller]")]
[ApiController]
public class FakeDataController : ControllerBase
{
    private readonly MudBlazorDbContext _context;
    public FakeDataController(MudBlazorDbContext context)
    {
        _context = context;
    }

    // GET: api/Values
    [HttpGet]
    public async Task<ActionResult<IEnumerable<CalendarEvent>>> CalendarEvents()
    {
        var fakedata = new List<CalendarEvent>
        {
            new CalendarEvent { Id = 1, Title = "Event 1", Description = "Description 1", TimeStamp = DateTime.Now },
            new CalendarEvent { Id = 2, Title = "Event 2", Description = "Description 2", TimeStamp = DateTime.Now.AddDays(1) },
            new CalendarEvent { Id = 3, Title = "Event 3", Description = "Description 3", TimeStamp = DateTime.Now.AddDays(2) }
        };
        return Ok(fakedata);
    }
}