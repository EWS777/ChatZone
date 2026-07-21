using ChatZone.Shared.DTOs;
using MediatR;

namespace ChatZone.Features.GroupMembers.GetList;

public class GetGroupMemberRequest : IRequest<Result<List<GetGroupMemberResponse>>>
{
    public int IdPerson { get; set; }
    public int IdGroup { get; set; }
    public DateTimeOffset? Cursor { get; set; }
    public int TakePerson { get; set; }
}