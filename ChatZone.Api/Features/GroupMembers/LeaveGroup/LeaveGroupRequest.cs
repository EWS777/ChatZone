using ChatZone.Shared.DTOs;
using MediatR;

namespace ChatZone.Features.GroupMembers.LeaveGroup;

public class LeaveGroupRequest : IRequest<Result<bool>>
{
    public int IdPerson { get; set; }
    public int IdChat { get; set; }
}