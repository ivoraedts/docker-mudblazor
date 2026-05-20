using Microsoft.AspNetCore.Mvc;
using MyApplication.Configuration;

namespace MyApplication.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AdminLinksController : ControllerBase
{
    private readonly IConfiguration _configuration;
    private readonly string _version;

    public AdminLinksController(IConfiguration configuration)
    {
        _configuration = configuration;
        _version = GetVersionString();
    }

    private string GetVersionString()
    {
        try
        {
            var assembly = System.Reflection.Assembly.GetExecutingAssembly();
            var versionAttribute = assembly.GetCustomAttributes(typeof(System.Reflection.AssemblyInformationalVersionAttribute), false)
                .OfType<System.Reflection.AssemblyInformationalVersionAttribute>()
                .FirstOrDefault();
            var versionString = versionAttribute?.InformationalVersion ?? "Unknown Version";
            
            // The metadata looks like: "1.0.0+build2026-05-20 14:56:00"
            var index = versionString.IndexOf("+build");
            if (index > 0)
            {
                return versionString.Substring(index + 6) + " UTC";
            }
            return versionString; //this should not happen

        }
        catch
        {
            return "Unknown Version"; //this should not happen
        }
    }

    [HttpGet]
    public ActionResult<AdminLinks> Get()
    {
        var adminLinks = new AdminLinks
        {
            DbAdminUrl = _configuration["AdminLinks:DbAdminUrl"]?.Trim(),
            MinioConsoleUrl = _configuration["AdminLinks:MinioConsoleUrl"]?.Trim(),
            Version = _version
        };

        return Ok(adminLinks);
    }
}
