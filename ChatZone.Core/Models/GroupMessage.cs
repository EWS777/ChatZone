namespace ChatZone.Core.Models;

public class GroupMessage
{
    public int IdGroupMessage { get; set; }
    public required string Message { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public int IdSender { get; set; }
    public int IdChat { get; set; }
    public Person Person { get; set; }
    public GroupChat GroupChat { get; set; }
    public Report Report { get; set; }
}