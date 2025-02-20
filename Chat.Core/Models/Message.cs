namespace Chat.Core.Models;

public class Message
{
    public int MessageId { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public string Text { get; set; }
    
    public int PersonId { get; set; }
    public Person Person { get; set; }
    public int ChatId { get; set; }
    public Chat Chat { get; set; }
}