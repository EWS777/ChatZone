using ChatZone.Core.Extensions;
using MediatR;

namespace ChatZone.Features.GroupChats.GetList;

public class GetGroupChatsRequest : IRequest<Result<List<GetGroupChatsResponse>>>
{
    
}