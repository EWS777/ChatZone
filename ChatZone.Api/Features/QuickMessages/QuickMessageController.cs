using System.Security.Claims;
using ChatZone.Core.Extensions.Exceptions;
using ChatZone.Features.QuickMessages.Create;
using ChatZone.Features.QuickMessages.GetList;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ChatZone.Features.QuickMessages;

[ApiController]
[Route("[controller]")]
public class QuickMessageController(IMediator mediator) : ControllerBase
{
    [Authorize(Roles = "User")]
    [HttpGet]
    [Route("{username}/quick-messages")]
    public async Task<List<GetQuickMessageListResponse>> GetQuickMessages(string username, CancellationToken cancellationToken)
    {
        var tokenUsername = User.FindFirst(ClaimTypes.Name)?.Value;
        var id = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (tokenUsername is null || id is null) throw new Exception("User does not exist!");
        if (tokenUsername != username) throw new ForbiddenAccessException("You are not an owner!");

        var result = await mediator.Send(new GetQuickMessageListRequest{Id = int.Parse(id)}, cancellationToken);
        return result.Match(x => x, x => throw x);
    }
    
    [Authorize(Roles = "User")]
    [HttpPost]
    [Route("{username}/quick-message")]
    public async Task<CreateQuickMessageResponse> CreateQuickMessage(string username, [FromBody] CreateQuickMessageRequest quickMessageRequest, CancellationToken cancellationToken)
    { 
        var tokenUsername = User.FindFirst(ClaimTypes.Name)?.Value;
        var id = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        
        if (tokenUsername is null || id is null) throw new NotFoundException("User does not exist!");
        if (tokenUsername != username) throw new ForbiddenAccessException("You are not an owner!");

        var result = await mediator.Send(quickMessageRequest, cancellationToken);
        return result.Match<CreateQuickMessageResponse>(x => x, x => throw x);
    }
}