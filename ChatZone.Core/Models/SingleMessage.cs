namespace ChatZone.Core.Models;

public class SingleMessage
{
    public int IdSingleMessage { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public required string Message { get; set; }
    public int IdSender { get; set; }
    public int IdChat { get; set; }
    public SingleChat SingleChat { get; set; }
    public Person Sender { get; set; }

    public Report Report { get; set; }
}