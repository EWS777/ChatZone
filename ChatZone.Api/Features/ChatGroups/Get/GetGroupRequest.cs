using ChatZone.Core.Extensions;
using MediatR;

namespace ChatZone.Features.ChatGroups.Get;

public class GetGroupRequest : IRequest<Result<GetGroupResponse>>
{
    public int IdPerson { get; set; }
    public int IdGroup { get; set; }
}