namespace ChatZone.Core.Models;

public class BlockedPerson
{
    public int IdBlockerPerson { get; set; }
    public int IdBlockedPerson { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public Person Blocker { get; set; }
    public Person Blocked { get; set; }
}