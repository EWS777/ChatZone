namespace ChatZone.Core.Models;

public class BlockedPeople
{
    public int BlockerPersonId { get; set; }
    public required Person Blocker { get; set; }
    public int BlockedPersonId { get; set; }
    public required Person Blocked { get; set; }
}