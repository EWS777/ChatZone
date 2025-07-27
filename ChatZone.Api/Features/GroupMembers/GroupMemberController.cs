using System.Security.Claims;
using ChatZone.Features.GroupMembers.Add;
using ChatZone.Features.GroupMembers.LeaveGroup;
using ChatZone.Features.GroupMembers.GetList;
using ChatZone.Features.GroupMembers.ChangeAdmin;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ChatZone.Features.GroupMembers;

[ApiController]
[Authorize(Roles = "User")]
[Route("[controller]")]
public class GroupMemberController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    [Route("get-list")]
    public async Task<List<GetGroupMemberResponse>> GetGroupMember([FromQuery] string groupName, CancellationToken cancellationToken)
    {
        var idPerson = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var groupMember = new GetGroupMemberRequest
        {
            IdPerson = int.Parse(idPerson!),
            IdGroup = int.Parse(groupName)
        };

        var result = await mediator.Send(groupMember, cancellationToken);
        return result.Match(x => x, x => throw x);
    }
    
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
    [Route("leave")]
    public async Task<IActionResult> LeaveGroup(CancellationToken cancellationToken)
    {
        var idPerson = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var groupMember = new LeaveGroupRequest
        {
            IdPerson = int.Parse(idPerson!)
        };

        var result = await mediator.Send(groupMember, cancellationToken);
        return result.Match(x => x, x => throw x);
    }
    
    [HttpPut]
    [Route("change-admin")]
    public async Task<IActionResult> ChangeAdmin([FromBody] ChangeAdminRequest request, CancellationToken cancellationToken)
    {
        var personId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (personId is null) throw new Exception("User does not exist!");

        request.IdPerson = int.Parse(personId);
        
        var result = await mediator.Send(request, cancellationToken);
        return result.Match(x => x, x => throw x);
    }
}