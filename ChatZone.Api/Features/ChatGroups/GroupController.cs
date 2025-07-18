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
}