using ChatZone.Core.Models.Enums;

namespace ChatZone.Core.Models;

public class GroupChat
{
    public int IdGroupChat { get; set; }
    public required string Title { get; set; }
    public int? IdCountry { get; set; }
    public Country? Country { get; set; }
    public int? IdCity { get; set; }
    public City? City { get; set; }
    public AgeList? Age { get; set; }
    public int? IdLanguage { get; set; }
    public Language? Language { get; set; }
    public int UserCount { set; get; }
    public DateTimeOffset CreatedAt { get; set; }
    public ICollection<GroupMember> GroupMembers { get; set; } = new List<GroupMember>();
    public ICollection<GroupMessage> GroupMessages { get; set; } = new List<GroupMessage>();
    public ICollection<BlockedGroupMember> BlockedGroupMembers { get; set; } = new List<BlockedGroupMember>();
}