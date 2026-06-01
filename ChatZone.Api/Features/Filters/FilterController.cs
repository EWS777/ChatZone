using System.Security.Claims;
using ChatZone.Features.Filters.Get;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ChatZone.Features.Filters;

[ApiController]
[Route("[controller]")]
public class FilterController(IMediator mediator) : ControllerBase
{
    [Authorize(Roles = "User")]
    [HttpGet]
    [Route("")]
    public async Task<GetFilterResponse> GetFilter(CancellationToken cancellationToken)
    {
        var idPerson = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (idPerson is null) throw new Exception("User does not exist!");
        var result = await mediator.Send(new GetFilterRequest{IdPerson = int.Parse(idPerson)}, cancellationToken);
        return result.Match<GetFilterResponse>(e => e, x=> throw x);
    }
}