using Microsoft.AspNetCore.Mvc;
using MyApplication.Data;
using MyApplication.Constants;
using Amazon.S3;
using Amazon.S3.Model;
using System.Reflection.Metadata;
using MyApplication.Configuration;

[Route("api/[controller]")]
[ApiController]
public class CalendarEventController : ControllerBase
{
    private readonly MudBlazorDbContext _context;
    private readonly IConfiguration _configuration;
    public CalendarEventController(MudBlazorDbContext context, IConfiguration configuration)
    {
        _context = context;
        _configuration = configuration;
        s3Configuration = GetS3Configuration();
    }

    private S3Configuration s3Configuration;
    private S3Configuration GetS3Configuration()
    {
        var s3Config = new S3Configuration
        {
            ServiceURL = _configuration.GetValue<string>("S3:ServiceURL") ?? "http://minio:9000",
            AccessKey = _configuration.GetValue<string>("S3:AccessKey") ?? "minioadmin",
            SecretKey = _configuration.GetValue<string>("S3:SecretKey") ?? "minioadmin",
            PublicURL = _configuration.GetValue<string>("S3:PublicURL") ?? "http://localhost:9000/"
        };
        return s3Config;
    }

    // GET: api/Values
    [HttpGet]
    public async Task<ActionResult<IEnumerable<CalendarEvent>>> CalendarEvents()
    {
        return await _context.CalendarEvents.AsAsyncEnumerable()
        .OrderBy(e => e.TimeStamp)
        .Select(e => e.ReplaceS3PrefixWithPublicURL(s3Configuration.PublicURL))
        .ToListAsync();
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
        if (calendarEvent.ImageFilePath != null)
        {
            calendarEvent.ImageFilePath = calendarEvent.ImageFilePath.Replace(Constants.S3StoragePrefix, s3Configuration.PublicURL);
        }
        return Ok(calendarEvent);
    }

    // POST: api/Values
    [HttpPost]
    public async Task<ActionResult<CalendarEvent>> PostCalendarEvent([FromForm] string title, [FromForm] string titleColor, [FromForm] string description, [FromForm] DateTime timeStamp, [FromForm] IFormFile? file)
    {        
        var calendarEvent = new CalendarEvent
        {
            Title = title,
            TitleColor = titleColor,
            Description = description,
            TimeStamp = timeStamp
        };
        
        if (file != null)
        {
            var s3Client = new AmazonS3Client(
                awsAccessKeyId: s3Configuration.AccessKey,
                awsSecretAccessKey: s3Configuration.SecretKey,
                new AmazonS3Config 
                { 
                    ServiceURL = s3Configuration.ServiceURL,
                    ForcePathStyle = true
                }
            );

            var bucketName = Constants.CalendarEventsBucketName;
            
            // Create bucket if it doesn't exist
            var listBucketsResponse = await s3Client.ListBucketsAsync();
            if (!listBucketsResponse.Buckets.Any(b => b.BucketName == bucketName))
            {
                await s3Client.PutBucketAsync(new PutBucketRequest { BucketName = bucketName });
            }

            var uniqueFileName = $"{Guid.NewGuid()}_{file.FileName}";
            
            using (var stream = file.OpenReadStream())
            {
                await s3Client.PutObjectAsync(new PutObjectRequest
                {
                    BucketName = bucketName,
                    Key = uniqueFileName,
                    InputStream = stream,
                    ContentType = file.ContentType
                });
            }

            calendarEvent.ImageFilePath = $"{Constants.S3StoragePrefix}{bucketName}/{uniqueFileName}";
        }
        
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
public static class S3ReplacePrefixExtensions
{
    public static CalendarEvent ReplaceS3PrefixWithPublicURL(this CalendarEvent calendarEvent, string publicURL)
    {
        if (calendarEvent.ImageFilePath != null)
        {
            calendarEvent.ImageFilePath = calendarEvent.ImageFilePath.Replace(Constants.S3StoragePrefix, publicURL);
        }
        return calendarEvent;
    }
}