using System.Security.Claims;
using ChatZone.Core.Extensions.Exceptions;
using ChatZone.Features.BlockedPersons.Create;
using ChatZone.Features.BlockedPersons.Delete;
using ChatZone.Features.BlockedPersons.GetList;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ChatZone.Features.BlockedPersons;

[ApiController]
[Route("[controller]")]
public class BlockPersonController(IMediator mediator) : ControllerBase
{
    [Authorize(Roles = "User")]
    [HttpGet]
    [Route("{username}/blocked-users")]
    public async Task<List<GetBlockedPersonsResponse>> GetBlockedPersons(string username, CancellationToken cancellationToken)
    {
        var tokenUsername = User.FindFirst(ClaimTypes.Name)?.Value;
        var id = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (tokenUsername is null || id is null) throw new Exception("User does not exist!");
        if (tokenUsername != username) throw new ForbiddenAccessException("You are not an owner!");

        var result = await mediator.Send(new GetBlockedPersonsRequest{Id = int.Parse(id)}, cancellationToken);
        return result.Match(x => x, x=>throw x);
    }

    [Authorize]
    [HttpPost]
    [Route("{username}/blocked-users/add")]
    public async Task<IActionResult> CreateBlockedPerson(string username, [FromRoute] int idBlockedPerson, CancellationToken cancellationToken)
    {
        var tokenUsername = User.FindFirst(ClaimTypes.Name)?.Value;
        var id = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (tokenUsername is null || id is null) throw new Exception("User does not exist!");
        if (tokenUsername != username) throw new ForbiddenAccessException("You are not an owner!");

        var result = await mediator.Send(new CreateBlockedPersonRequest { IdPerson = int.Parse(id), IdBlockedPerson = idBlockedPerson }, cancellationToken);
        return result.Match(x => x, x => throw x);
    }
    
    [Authorize(Roles = "User")]
    [HttpDelete]
    [Route("{username}/blocked-users/{idBlockedPerson}")]
    public async Task<IActionResult> DeleteBlockedPerson(string username, [FromRoute] int idBlockedPerson, CancellationToken cancellationToken)
    {
        var tokenUsername = User.FindFirst(ClaimTypes.Name)?.Value;
        var id = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (tokenUsername is null || id is null) throw new Exception("User does not exist!");
        if (tokenUsername != username) throw new ForbiddenAccessException("You are not an owner!");
        
        var result = await mediator.Send(new DeleteBlockedPersonRequest{Id = int.Parse(id), IdBlockedPerson = idBlockedPerson}, cancellationToken);
        return result.Match<IActionResult>(x=>x, x => throw x);
    }
}