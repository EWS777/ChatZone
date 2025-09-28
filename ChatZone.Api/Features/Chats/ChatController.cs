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
    public async Task<GetActiveChatResponse> GetActiveChat(CancellationToken cancellationToken)
    {
        var idPerson = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (idPerson is null) throw new Exception("User does not exist!");
        
        return await mediator.Send(new GetActiveChatRequest{IdPerson = int.Parse(idPerson)}, cancellationToken);
    }
    
    [HttpGet]
    [Route("")]
    public async Task<GetChatPersonInfoResponse> GetChatPersonInfo(CancellationToken cancellationToken)
    {
        var idPerson = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (idPerson is null) throw new Exception("User does not exist!");
        var result = await mediator.Send(new GetChatPersonInfoRequest{IdPerson = int.Parse(idPerson)}, cancellationToken);
        return result.Match<GetChatPersonInfoResponse>(e => e, x=> throw x);
    }
    
    
    [HttpGet]
    [Route("get-group")]
    public async Task<GetGroupChatResponse> GetGroupChat([FromQuery] int idGroup, CancellationToken cancellationToken)
    {
        var personId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (personId is null) throw new Exception("User does not exist!");
        
        var result = await mediator.Send(new GetGroupChatRequest{IdGroup = idGroup, IdPerson = int.Parse(personId)}, cancellationToken);
        return result.Match(x => x, x => throw x);
    }
    
    [HttpGet]
    [Route("get")]
    public async Task<List<GetGroupChatsResponse>> GetGroupChats(CancellationToken cancellationToken)
    {
        var personId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (personId is null) throw new Exception("User does not exist!");
        
        var result = await mediator.Send(new GetGroupChatsRequest{IdPerson = int.Parse(personId)}, cancellationToken);
        return result.Match(x => x, x => throw x);
    }
    
    [HttpPost]
    [Route("create")]
    public async Task<int> CreateGroupChat([FromBody] CreateGroupChatRequest request, CancellationToken cancellationToken)
    {
        var personId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (personId is null) throw new Exception("User does not exist!");

        request.IdPerson = int.Parse(personId);
        
        var result = await mediator.Send(request, cancellationToken);
        return result.Match(x => x, x => throw x);
    }
    
    [HttpPut]
    [Route("update")]
    public async Task<UpdateGroupChatResponse> UpdateGroupChat([FromBody] UpdateGroupChatRequest request, CancellationToken cancellationToken)
    {
        var personId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (personId is null) throw new Exception("User does not exist!");

        request.IdPerson = int.Parse(personId);
        
        var result = await mediator.Send(request, cancellationToken);
        return result.Match(x => x, x => throw x);
    }
    
    [HttpDelete]
    [Route("delete")]
    public async Task<IActionResult> DeleteGroupChat([FromQuery] int idGroup, CancellationToken cancellationToken)
    {
        var personId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (personId is null) throw new Exception("User does not exist!");

        var request = new DeleteGroupChatRequest
        {
            IdPerson = int.Parse(personId),
            IdGroup = idGroup
        };
        
        var result = await mediator.Send(request, cancellationToken);
        return result.Match(x => x, x => throw x);
    }

    [HttpPut]
    [Route("finish")]
    public async Task FinishSingleChat([FromQuery] int idChat, CancellationToken cancellationToken)
    {
        await mediator.Send(new FinishSingleChatRequest{IdChat = idChat}, cancellationToken);
    }
}