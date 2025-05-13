namespace ChatZone.Core.Models;

public class GroupMember
{
    public int IdChat { get; set; }
    public int IdGroupMember { get; set; }
    public bool IsAdmin { get; set; }
    public DateTime JoinedAt { get; set; }
    public required Person Person { get; set; }
    public required GroupChat GroupChat { get; set; }
}