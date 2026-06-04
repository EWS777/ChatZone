using System.Security.Claims;
using ChatZone.Features.BlockedGroupMembers.Add;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ChatZone.Features.BlockedGroupMembers;

[ApiController]
[Route("[controller]")]
public class BlockedGroupMemberController(IMediator mediator) : ControllerBase
{
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "User")]
    [HttpPost]
    [Route("add")]
    public async Task<IActionResult> AddBlockedGroupMember([FromBody] AddBlockedGroupRequest request, CancellationToken cancellationToken)
    {
        var idPerson = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (idPerson is null) return Unauthorized(new { message = "You are not authorized!" });

        request.IdAdminPerson = int.Parse(idPerson);

        var result = await mediator.Send(request, cancellationToken);
        return result.Match(_ => Ok(new {message = "Person has blocked successfully!"}), x => throw x);
    }
}