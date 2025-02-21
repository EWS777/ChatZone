namespace ChatZone.Core.Models;

public class QuickMessage
{
    public required string Message { get; set; }
    public int PersonId { get; set; }
    public required Person Person { get; set; }
}