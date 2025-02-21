namespace ChatZone.Core.Models;

public class ChatMember
{
    public bool IsAdmin { get; set; }
    public DateTimeOffset JoinedAt { get; set; }
    public int PersonMemberId { get; set; }
    public required Person Person { get; set; }
    public int GroupChatId { get; set; }
    public required GroupChat GroupChat { get; set; }
}