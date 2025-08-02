namespace ChatZone.Core.Models;

public class BlockedGroupMember
{
    public int IdChat { get; set; }
    public int IdBlockedPerson { get; set; }
    public DateTimeOffset BlockedAt { get; set; }
    public Person Person { get; set; }
    public GroupChat GroupChat { get; set; }
}