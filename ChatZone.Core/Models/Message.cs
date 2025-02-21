namespace ChatZone.Core.Models;

public class Message
{
    public int MessageId { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public required string Text { get; set; }
    public int PersonId { get; set; }
    public required Person Person { get; set; }
    public int ChatId { get; set; }
    public required Chat Chat { get; set; }
}