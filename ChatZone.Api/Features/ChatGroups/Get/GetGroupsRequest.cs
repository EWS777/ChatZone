using ChatZone.Core.Extensions;
using MediatR;

namespace ChatZone.Features.ChatGroups.Get;

public class GetGroupsRequest : IRequest<Result<List<GetGroupsResponse>>>
{
    
}