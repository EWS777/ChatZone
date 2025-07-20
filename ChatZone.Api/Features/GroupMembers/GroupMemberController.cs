using System.Security.Claims;
using ChatZone.Features.GroupMembers.Add;
using ChatZone.Features.GroupMembers.Delete;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ChatZone.Features.GroupMembers;

[ApiController]
[Authorize(Roles = "User")]
[Route("[controller]")]
public class GroupMemberController(IMediator mediator) : ControllerBase
{
    [HttpPost]
    [Route("add")]
    public async Task<IActionResult> AddGroupMember([FromQuery] int groupName, CancellationToken cancellationToken)
    {
        var idPerson = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var groupMember = new AddGroupMemberRequest
        {
            IdPerson = int.Parse(idPerson!),
            IdGroup = groupName
        };

        var result = await mediator.Send(groupMember, cancellationToken);
        return result.Match(x => x, x => throw x);
    }

    [HttpDelete]
    [Route("delete")]
    public async Task<IActionResult> DeleteGroupMember(CancellationToken cancellationToken)
    {
        var idPerson = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var groupMember = new DeleteGroupMemberRequest
        {
            IdPerson = int.Parse(idPerson!)
        };

        var result = await mediator.Send(groupMember, cancellationToken);
        return result.Match(x => x, x => throw x);
    }
}