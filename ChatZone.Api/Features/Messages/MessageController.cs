using System.Security.Claims;
using ChatZone.Features.Messages.Get;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ChatZone.Features.Messages;

[ApiController]
[Route("[controller]")]
public class MessageController(IMediator mediator) : ControllerBase
{
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "User")]
    [Route("get")]
    [HttpPost]
    public async Task<ActionResult<GetMessageResponse>> GetMessages([FromBody] GetMessageRequest request, CancellationToken cancellationToken)
    {
        var idPerson = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (idPerson is null) return Unauthorized(new { message = "You are not authorized!" });

        request.IdPerson = int.Parse(idPerson);
        var result = await mediator.Send(request, cancellationToken);
        return result.Match(x => x, x => throw x);
    }
}