using ChatZone.Shared.DTOs;
using MediatR;

namespace ChatZone.Features.Chats.GroupChats.GetList;

public class GetGroupChatsRequest : IRequest<Result<List<GetGroupChatsResponse>>>
{
    public int IdPerson { get; set; }
    public DateTimeOffset? Cursor { get; set; }
    public int? CursorId { get; set; }
    public int TakeGroup { get; set; }
}