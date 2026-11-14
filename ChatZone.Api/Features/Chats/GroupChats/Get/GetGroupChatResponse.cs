using ChatZone.Core.Models.Enums;
using ChatZone.Shared.DTOs;

namespace ChatZone.Features.Chats.GroupChats.Get;

public class GetGroupChatResponse
{
    public int IdGroup { get; set; }
    public required string Title { get; set; }
    public NamedItemDto? Country { get; set; }
    public NamedItemDto? City { get; set; }
    public AgeList? Age { get; set; }
    public NamedItemDto? Language { get; set; }
    public bool IsAdmin { get; set; }
    public int UserCount { get; set; }
}