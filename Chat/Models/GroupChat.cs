using Chat.Models.Enums;

namespace Chat.Models;

public class GroupChat : Chat
{
    public string Title { get; set; }
    public CountryList Country { get; set; }
    public CityList City { get; set; }
    public AgeList Age { get; set; }
    public LangList Lang { get; set; }
    public int UserCount { get; set; }
    
    public int GroupChatId { get; set; }
    public Chat Chat { get; set; }
    public ICollection<ChatMember> ChatMembers = new List<ChatMember>();
}