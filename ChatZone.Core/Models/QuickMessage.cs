namespace ChatZone.Core.Models;

public class QuickMessage
{
    public int IdQuickMessage { get; set; }
    public required string Message { get; set; }
    public int IdPerson { get; set; }
    public Person Person { get; set; }
}