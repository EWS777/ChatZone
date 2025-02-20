namespace Chat.Core.Models;

public class QuickMessage
{
    public string Message { get; set; }
    
    public int PersonId { get; set; }
    public Person Person { get; set; }
}