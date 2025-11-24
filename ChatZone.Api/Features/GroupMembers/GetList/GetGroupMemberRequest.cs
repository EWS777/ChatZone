using ChatZone.Shared.DTOs;
using MediatR;

namespace ChatZone.Features.GroupMembers.GetList;

public class GetGroupMemberRequest : IRequest<Result<List<GetGroupMemberResponse>>>
{
    public int IdPerson { get; set; }
    public int IdGroup { get; set; }
}