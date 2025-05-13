namespace ChatZone.Core.Models;

public class BlockedPerson
{
    public int IdBlockerPerson { get; set; }
    public int IdBlockedPerson { get; set; }
    public DateTime CreatedAt { get; set; }
    public required Person Blocker { get; set; }
    public required Person Blocked { get; set; }
}