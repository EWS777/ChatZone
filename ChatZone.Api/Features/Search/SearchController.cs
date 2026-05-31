using System.Security.Claims;
using ChatZone.Features.Search.Cancel;
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
    [Route("cancel")]
    public async Task<IActionResult> Cancel(CancellationToken cancellationToken)
    {
        var personId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var result = await mediator.Send(new CancelPersonRequest{IdPerson = int.Parse(personId!)}, cancellationToken);
        return result.Match(x => Ok(new {message = "Cancel finding is successful!"}), x=> throw x);
    }
}