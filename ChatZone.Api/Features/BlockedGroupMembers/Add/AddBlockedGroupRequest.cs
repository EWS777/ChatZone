using ChatZone.Shared.DTOs;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ChatZone.Features.BlockedGroupMembers.Add;

public class AddBlockedGroupRequest : IRequest<Result<bool>>
{
    public int IdAdminPerson { get; set; }
    public int IdBlockedPerson { get; set; }
    public int IdChat { get; set; }
}