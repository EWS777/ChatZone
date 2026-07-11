using ChatZone.Shared.DTOs;
using MediatR;

namespace ChatZone.Features.GroupMembers.Add;

public class AddGroupMemberRequest : IRequest<Result<bool>>
{
    public int IdPerson { get; set; }
    public int IdGroup { get; set; }
}