namespace MyApplication.Client.Models
{
    public class CalendarEvent
    {
        public int Id {get; set;}
        public required string Title {get; set;} = string.Empty;
        public required string Description {get; set;} = string.Empty;
    }
}