using System.Security.Claims;
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
    [Route("")]
    public async Task<ActionResult<List<GetQuickMessageListResponse>>> GetQuickMessages(CancellationToken cancellationToken)
    {
        var idPerson = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (idPerson is null) return Unauthorized(new { message = "You are not authorized!" });

        var result = await mediator.Send(new GetQuickMessageListRequest{IdPerson = int.Parse(idPerson)}, cancellationToken);
        return result.Match(x => x, x => throw x);
    }
    
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "User")]
    [HttpPost]
    [Route("add")]
    public async Task<ActionResult<CreateQuickMessageResponse>> CreateQuickMessage([FromBody] CreateQuickMessageRequest quickMessageRequest, CancellationToken cancellationToken)
    {
        var idPerson = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (idPerson is null) return Unauthorized(new { message = "You are not authorized!" });
        
        quickMessageRequest.IdPerson = int.Parse(idPerson);

        var result = await mediator.Send(quickMessageRequest, cancellationToken);
        return result.Match<CreateQuickMessageResponse>(x => x, x => throw x);
    }

    [ValidateAntiForgeryToken]
    [Authorize(Roles = "User")]
    [HttpPut]
    [Route("{id}/update")]
    public async Task<ActionResult<UpdateQuickMessageResponse>> UpdateQuickMessage([FromRoute] int id,
        [FromBody] UpdateQuickMessageRequest request, CancellationToken cancellationToken)
    {
        var idPerson = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        
        if (idPerson is null) return Unauthorized(new { message = "You are not authorized!" });
        
        request.IdPerson = int.Parse(idPerson);
        var result = await mediator.Send(request, cancellationToken);
        return result.Match<UpdateQuickMessageResponse>(x => x, x => throw x);
    }

    [ValidateAntiForgeryToken]
    [Authorize(Roles = "User")]
    [HttpDelete]
    [Route("delete/{idQuickMessage:int}")]
    public async Task<IActionResult> DeleteQuickMessage([FromRoute] int idQuickMessage, CancellationToken cancellationToken)
    {
        var idPerson = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        
        if (idPerson is null) return Unauthorized(new { message = "You are not authorized!" });
        
        var result = await mediator.Send(new DeleteQuickMessageRequest{IdPerson = int.Parse(idPerson), IdMessage = idQuickMessage}, cancellationToken);
        return result.Match<IActionResult>(_ => Ok(new {message = "Quick message was deleted successfully!"}), x => throw x);
    }
}