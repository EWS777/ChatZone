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
    [Authorize("User")]
    [Route("get")]
    [HttpGet]
    public async Task<GetMessageResponse> GetMessages([FromBody] GetMessageRequest request, CancellationToken cancellationToken)
    {
        var idPerson = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (idPerson is null) throw new Exception("User doesn't exist!");

        request.IdPerson = int.Parse(idPerson);
        var result = await mediator.Send(request, cancellationToken);
        return result.Match(x => x, x => throw x);
    }
}