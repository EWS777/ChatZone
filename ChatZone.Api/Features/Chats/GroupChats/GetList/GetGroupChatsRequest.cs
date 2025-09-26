using ChatZone.Core.Extensions;
using MediatR;

namespace ChatZone.Features.Chats.GroupChats.GetList;

public class GetGroupChatsRequest : IRequest<Result<List<GetGroupChatsResponse>>>
{
    public int IdPerson { get; set; }
}