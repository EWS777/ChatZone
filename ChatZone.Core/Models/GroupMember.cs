namespace ChatZone.Core.Models;

public class GroupMember
{
    public int IdChat { get; set; }
    public int IdGroupMember { get; set; }
    public bool IsAdmin { get; set; }
    public DateTimeOffset JoinedAt { get; set; }
    public Person Person { get; set; }
    public GroupChat GroupChat { get; set; }
}