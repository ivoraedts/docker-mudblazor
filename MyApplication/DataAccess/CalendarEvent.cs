namespace MyApplication.Data
{
    public class CalendarEvent
    {
        public int Id {get; set;}
        public required string Title {get; set;}
        public required string TitleColor {get; set;}
        public required string Description {get; set;}
        public required DateTime TimeStamp {get; set;}
        public string? FilePath {get; set;}
    }
}