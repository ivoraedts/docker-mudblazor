namespace MyApplication.Client.Models
{
    public class CalendarEvent
    {
        public int Id {get; set;}
        public required string Title {get; set;} = string.Empty;
        public required string TitleColor {get; set;} = "#db1f1fff";
        public required string Description {get; set;} = string.Empty;
        public required DateTime TimeStamp {get; set;}
    }
}