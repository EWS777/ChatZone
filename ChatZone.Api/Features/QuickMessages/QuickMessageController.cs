using System.Security.Claims;
using ChatZone.Core.Extensions.Exceptions;
using ChatZone.Features.QuickMessages.Create;
using ChatZone.Features.QuickMessages.Delete;
using ChatZone.Features.QuickMessages.GetList;
using ChatZone.Features.QuickMessages.Update;
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

    [Authorize(Roles = "User")]
    [HttpPut]
    [Route("{username}/quick-message/{id}/update")]
    public async Task<UpdateQuickMessageResponse> UpdateQuickMessage(string username, int id,
        [FromBody] UpdateQuickMessageRequest request, CancellationToken cancellationToken)
    {
        var tokenUsername = User.FindFirst(ClaimTypes.Name)?.Value;
        var idPerson = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        
        if (tokenUsername is null || idPerson is null) throw new NotFoundException("User does not exist!");
        if (tokenUsername != username) throw new ForbiddenAccessException("You are not an owner!");
        
        request.IdPerson = int.Parse(idPerson);
        var result = await mediator.Send(request, cancellationToken);
        return result.Match<UpdateQuickMessageResponse>(x => x, x => throw x);
    }

    [Authorize(Roles = "User")]
    [HttpDelete]
    [Route("{username}/quick-message")]
    public async Task<IActionResult> DeleteQuickMessage(string username, [FromQuery] int idQuickMessage, CancellationToken cancellationToken)
    {
        var tokenUsername = User.FindFirst(ClaimTypes.Name)?.Value;
        var id = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        
        if (tokenUsername is null || id is null) throw new NotFoundException("User does not exist!");
        if (tokenUsername != username) throw new ForbiddenAccessException("You are not an owner!");
        
        var result = await mediator.Send(new DeleteQuickMessageRequest{IdPerson = int.Parse(id), IdMessage = idQuickMessage}, cancellationToken);
        return result.Match<IActionResult>(x => x, x => throw x);
    }
}