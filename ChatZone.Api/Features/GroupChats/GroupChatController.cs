using System.Security.Claims;
using ChatZone.Features.GroupChats.Create;
using ChatZone.Features.GroupChats.Delete;
using ChatZone.Features.GroupChats.Get;
using ChatZone.Features.GroupChats.GetList;
using ChatZone.Features.GroupChats.Update;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ChatZone.Features.GroupChats;

[ApiController]
[Authorize(Roles = "User")]
[Route("[controller]")]
public class GroupChatController(IMediator mediator) : ControllerBase
{
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
}