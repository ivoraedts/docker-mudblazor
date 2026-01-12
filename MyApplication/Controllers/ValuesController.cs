using Microsoft.AspNetCore.Mvc;
using MyApplication.Data;

[Microsoft.AspNetCore.Mvc.Route("api/[controller]")]
[ApiController]
public class ValuesController : ControllerBase
{
    private readonly MyApplication.Data.MudBlazorDbContext _context;
    public ValuesController(MyApplication.Data.MudBlazorDbContext context)
    {
        _context = context;
    }

    // GET: api/Values
    [HttpGet]
    public async Task<ActionResult<IEnumerable<CalendarEvent>>> CalendarEvents()
    {
        return await _context.CalendarEvents.AsAsyncEnumerable().ToListAsync();
    }
}