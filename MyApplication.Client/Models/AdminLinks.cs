namespace MyApplication.Client.Models
{
    public class AdminLinks
    {
        public string? DbAdminUrl { get; set; }
        public string? MinioConsoleUrl { get; set; }
        public string Version { get; set; } = string.Empty;
    }
}