using System.Security.Claims;
using ChatZone.Core.Extensions.Exceptions;
using ChatZone.Features.Filters.Get;
using ChatZone.Features.Filters.Update;
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
    [Route("{username}/filter")]
    public async Task<GetFilterResponse> GetFilter([FromRoute] string username, CancellationToken cancellationToken)
    {
        var tokenUsername = User.FindFirst(ClaimTypes.Name)?.Value;
        var id = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (tokenUsername is null || id is null) throw new Exception("User does not exist!");
        if (tokenUsername != username) throw new ForbiddenAccessException("You are not an owner!");

        var result = await mediator.Send(new GetFilterRequest{Id = int.Parse(id)}, cancellationToken);
        return result.Match<GetFilterResponse>(e => e, x=> throw x);
    }

    [Authorize(Roles = "User")]
    [HttpPut]
    [Route("{username}/filter")]
    public async Task<UpdateFilterResponse> UpdateFilter(string username, [FromBody] UpdateFilterRequest filterRequest, CancellationToken cancellationToken)
    {
        var tokenUsername = User.FindFirst(ClaimTypes.Name)?.Value;
        var id = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (tokenUsername is null || id is null) throw new Exception("User does not exist!");
        if (tokenUsername != username) throw new ForbiddenAccessException("You are not an owner!");
        filterRequest.Id = int.Parse(id);
        
        var result = await mediator.Send(filterRequest, cancellationToken);
        return result.Match<UpdateFilterResponse>(e => e, e => throw e);
    }
}