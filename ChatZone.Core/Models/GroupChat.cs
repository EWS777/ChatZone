using ChatZone.Core.Models.Enums;

namespace ChatZone.Core.Models;

public class GroupChat : Chat
{
    public required string Title { get; set; }
    public CountryList? Country { get; set; }
    public CityList? City { get; set; }
    public AgeList? Age { get; set; }
    public LangList? Lang { get; set; }
    public int UserCount { set; get; }
    public ICollection<ChatMember> ChatMembers { get; set; } = new List<ChatMember>();
}