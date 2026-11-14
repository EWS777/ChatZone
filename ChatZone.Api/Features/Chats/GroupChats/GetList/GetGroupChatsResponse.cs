using ChatZone.Core.Models.Enums;
using ChatZone.Shared.DTOs;

namespace ChatZone.Features.Chats.GroupChats.GetList;

public class GetGroupChatsResponse
{
    public int IdGroup { get; set; }
    public required string Title { get; set; }
    public int PersonCount { get; set; }
    public NamedItemDto? Country { get; set; }
    public NamedItemDto? City { get; set; }
    public AgeList? Age { get; set; }
    public NamedItemDto? Language { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
}