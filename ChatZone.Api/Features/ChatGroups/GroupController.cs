using System.Security.Claims;
using ChatZone.Features.ChatGroups.Create;
using ChatZone.Features.ChatGroups.Get;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ChatZone.Features.ChatGroups;

[ApiController]
[Authorize(Roles = "User")]
[Route("[controller]")]
public class GroupController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    [Route("get")]
    public async Task<List<GetGroupsResponse>> GetGroups(CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new GetGroupsRequest(), cancellationToken);
        return result.Match(x => x, x => throw x);
    }
    
    [HttpPost]
    [Route("create")]
    public async Task<int> CreateGroup([FromBody] CreateGroupRequest request, CancellationToken cancellationToken)
    {
        var personId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (personId is null) throw new Exception("User does not exist!");

        request.IdPerson = int.Parse(personId);
        
        var result = await mediator.Send(request, cancellationToken);
        return result.Match(x => x, x => throw x);
    }
}