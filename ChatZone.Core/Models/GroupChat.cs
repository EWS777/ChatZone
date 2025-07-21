using ChatZone.Core.Models.Enums;

namespace ChatZone.Core.Models;

public class GroupChat
{
    public int IdGroupChat { get; set; }
    public required string Title { get; set; }
    public CountryList? Country { get; set; }
    public CityList? City { get; set; }
    public AgeList? Age { get; set; }
    public LangList? Lang { get; set; }
    public int UserCount { set; get; }
    public ICollection<GroupMember> GroupMembers { get; set; } = new List<GroupMember>();
    public ICollection<GroupMessage> GroupMessages { get; set; } = new List<GroupMessage>();
}