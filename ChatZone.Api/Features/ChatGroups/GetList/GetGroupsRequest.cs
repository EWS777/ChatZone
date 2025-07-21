using ChatZone.Core.Extensions;
using MediatR;

namespace ChatZone.Features.ChatGroups.GetList;

public class GetGroupsRequest : IRequest<Result<List<GetGroupsResponse>>>
{
    
}