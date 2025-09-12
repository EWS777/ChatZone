using System.Security.Claims;
using ChatZone.Features.ChatInfo.Get;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ChatZone.Features.ChatInfo;

[ApiController]
[Route("[controller]")]
public class ChatInfoController(IMediator mediator) : ControllerBase
{
    [Authorize(Roles = "User")]
    [HttpGet]
    [Route("")]
    public async Task<GetChatPersonInfoResponse> GetChatPersonInfo(CancellationToken cancellationToken)
    {
        var idPerson = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (idPerson is null) throw new Exception("User does not exist!");
        var result = await mediator.Send(new GetChatPersonInfoRequest{IdPerson = int.Parse(idPerson)}, cancellationToken);
        return result.Match<GetChatPersonInfoResponse>(e => e, x=> throw x);
    }
}