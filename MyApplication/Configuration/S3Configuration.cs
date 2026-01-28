using System;

namespace MyApplication.Configuration;

public class S3Configuration
{
    public required string ServiceURL { get; set; }
    public required string AccessKey { get; set; }
    public required string SecretKey { get; set; }
    public required string PublicURL { get; set; }
}
