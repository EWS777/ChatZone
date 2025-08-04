using System.Security.Claims;
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
    [Route("")]
    public async Task<List<GetBlockedPersonsResponse>> GetBlockedPersons(CancellationToken cancellationToken)
    {
        var idPerson = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (idPerson is null) throw new Exception("User does not exist!");

        var result = await mediator.Send(new GetBlockedPersonsRequest{IdPerson = int.Parse(idPerson)}, cancellationToken);
        return result.Match(x => x, x=>throw x);
    }

    [Authorize]
    [HttpPost]
    [Route("add/{idPartnerPerson}")]
    public async Task<IActionResult> CreateBlockedPerson([FromRoute] int idPartnerPerson, CancellationToken cancellationToken)
    {
        var idPerson = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (idPerson is null) throw new Exception("User does not exist!");

        var result = await mediator.Send(new CreateBlockedPersonRequest { IdPerson = int.Parse(idPerson), IdPartnerPerson = idPartnerPerson }, cancellationToken);
        return result.Match(x => x, x => throw x);
    }
    
    [Authorize(Roles = "User")]
    [HttpDelete]
    [Route("delete/{idBlockedPerson}")]
    public async Task<IActionResult> DeleteBlockedPerson([FromRoute] int idBlockedPerson, CancellationToken cancellationToken)
    {
        var idPerson = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (idPerson is null) throw new Exception("User does not exist!");
        
        var result = await mediator.Send(new DeleteBlockedPersonRequest{IdPerson = int.Parse(idPerson), IdBlockedPerson = idBlockedPerson}, cancellationToken);
        return result.Match<IActionResult>(x=>x, x => throw x);
    }
}