using Microsoft.AspNetCore.Mvc;
using MyApplication.Configuration;

namespace MyApplication.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AdminLinksController : ControllerBase
{
    private readonly IConfiguration _configuration;

    public AdminLinksController(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    [HttpGet]
    public ActionResult<AdminLinks> Get()
    {
        var adminLinks = new AdminLinks
        {
            DbAdminUrl = _configuration["AdminLinks:DbAdminUrl"]?.Trim(),
            MinioConsoleUrl = _configuration["AdminLinks:MinioConsoleUrl"]?.Trim()
        };

        return Ok(adminLinks);
    }
}
