namespace Chat.Models;

public class BlockedPeople
{
    public int BlockerPersonId { get; set; }
    public Person Blocker { get; set; }
    
    public int BlockedPersonId { get; set; }
    public Person Blocked { get; set; }
}