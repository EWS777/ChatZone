using System.Security.Claims;
using ChatZone.Features.Search.Cancel;
using ChatZone.Features.Search.Find;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ChatZone.Features.Search;

[ApiController]
[Route("[controller]")]
[Authorize(Roles = "User")]
public class SearchController(IMediator mediator) : ControllerBase
{
    [HttpPost]
    [Route("search")]
    public async Task<IActionResult> FindPerson([FromBody] FindPersonRequest personRequest, CancellationToken cancellationToken)
    {
        var personId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        personRequest.IdPerson = int.Parse(personId!);
        var result = await mediator.Send(personRequest, cancellationToken);
        return result.Match(x => x, x=> throw x);
    }

    [HttpDelete]
    [Route("cancel")]
    public async Task<IActionResult> Cancel(CancellationToken cancellationToken)
    {
        var personId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var result = await mediator.Send(new CancelPersonRequest{IdPerson = int.Parse(personId!)}, cancellationToken);
        return result.Match(x => x, x=> throw x);
    }
}