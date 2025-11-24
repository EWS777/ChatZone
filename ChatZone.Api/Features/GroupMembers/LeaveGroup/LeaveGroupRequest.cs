using ChatZone.Shared.DTOs;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ChatZone.Features.GroupMembers.LeaveGroup;

public class LeaveGroupRequest : IRequest<Result<IActionResult>>
{
    public int IdPerson { get; set; }
    public int IdChat { get; set; }
}