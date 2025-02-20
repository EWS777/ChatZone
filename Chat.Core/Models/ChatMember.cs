namespace Chat.Core.Models;

public class ChatMember
{
    public bool IsAdmin { get; set; }
    public DateTimeOffset JoinedAt { get; set; }
    
    public int PersonMemberId { get; set; }
    public Person Person { get; set; }
    public int GroupChatId { get; set; }
    public GroupChat GroupChat { get; set; }
}