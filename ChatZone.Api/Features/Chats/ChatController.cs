using System.Security.Claims;
using ChatZone.Features.Chats.Common.Get;
using ChatZone.Features.Chats.Common.GetActiveChat;
using ChatZone.Features.Chats.GroupChats.Create;
using ChatZone.Features.Chats.GroupChats.Delete;
using ChatZone.Features.Chats.GroupChats.Get;
using ChatZone.Features.Chats.GroupChats.GetList;
using ChatZone.Features.Chats.GroupChats.Update;
using ChatZone.Features.Chats.SingleChats.Finish;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ChatZone.Features.Chats;

[ApiController]
[Authorize(Roles = "User")]
[Route("[controller]")]
public class ChatController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    [Route("active-chat")]
    public async Task<ActionResult<GetActiveChatResponse>> GetActiveChat(CancellationToken cancellationToken)
    {
        var idPerson = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (idPerson is null) return Unauthorized(new { message = "You are not authorized!" });
        
        var result = await mediator.Send(new GetActiveChatRequest{IdPerson = int.Parse(idPerson)}, cancellationToken);
        return result.Match(x => x, x => throw x);
    }
    
    [HttpGet]
    [Route("")]
    public async Task<ActionResult<GetChatPersonInfoResponse>> GetChatPersonInfo(CancellationToken cancellationToken)
    {
        var idPerson = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (idPerson is null) return Unauthorized(new { message = "You are not authorized!" });
        var result = await mediator.Send(new GetChatPersonInfoRequest{IdPerson = int.Parse(idPerson)}, cancellationToken);
        return result.Match<GetChatPersonInfoResponse>(e => e, x=> throw x);
    }
    
    
    [HttpGet]
    [Route("get-group")]
    public async Task<ActionResult<GetGroupChatResponse>> GetGroupChat([FromQuery] int idGroup, CancellationToken cancellationToken)
    {
        var idPerson = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (idPerson is null) return Unauthorized(new { message = "You are not authorized!" });
        
        var result = await mediator.Send(new GetGroupChatRequest{IdGroup = idGroup, IdPerson = int.Parse(idPerson)}, cancellationToken);
        return result.Match(x => x, x => throw x);
    }
    
    [HttpGet]
    [Route("get")]
    public async Task<ActionResult<List<GetGroupChatsResponse>>> GetGroupChats(CancellationToken cancellationToken)
    {
        var idPerson = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (idPerson is null) return Unauthorized(new { message = "You are not authorized!" });
        
        var result = await mediator.Send(new GetGroupChatsRequest{IdPerson = int.Parse(idPerson)}, cancellationToken);
        return result.Match(x => x, x => throw x);
    }
    
    [HttpPost]
    [Route("create")]
    public async Task<ActionResult<int>> CreateGroupChat([FromBody] CreateGroupChatRequest request, CancellationToken cancellationToken)
    {
        var idPerson = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (idPerson is null) return Unauthorized(new { message = "You are not authorized!" });

        request.IdPerson = int.Parse(idPerson);
        
        var result = await mediator.Send(request, cancellationToken);
        return result.Match(x => x, x => throw x);
    }
    
    [HttpPut]
    [Route("update")]
    public async Task<ActionResult<UpdateGroupChatResponse>> UpdateGroupChat([FromBody] UpdateGroupChatRequest request, CancellationToken cancellationToken)
    {
        var idPerson = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (idPerson is null) return Unauthorized(new { message = "You are not authorized!" });

        request.IdPerson = int.Parse(idPerson);
        
        var result = await mediator.Send(request, cancellationToken);
        return result.Match(x => x, x => throw x);
    }
    
    [HttpDelete]
    [Route("delete")]
    public async Task<IActionResult> DeleteGroupChat([FromQuery] int idGroup, CancellationToken cancellationToken)
    {
        var idPerson = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (idPerson is null) return Unauthorized(new { message = "You are not authorized!" });

        var request = new DeleteGroupChatRequest
        {
            IdPerson = int.Parse(idPerson),
            IdGroup = idGroup
        };
        
        var result = await mediator.Send(request, cancellationToken);
        return result.Match(x => Ok(new {message = "Group was deleted successfully!"}), x => throw x);
    }

    [HttpPut]
    [Route("finish")]
    public async Task<IActionResult> FinishSingleChat([FromQuery] int idChat, CancellationToken cancellationToken)
    {
        var idPerson = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (idPerson is null) return Unauthorized(new { message = "You are not authorized!" });
        
        var result = await mediator.Send(new FinishSingleChatRequest
        {
            IdChat = idChat,
            IdPerson = int.Parse(idPerson)
        }, cancellationToken);
        
        return result.Match(x => Ok(), x => throw x);
    }
}