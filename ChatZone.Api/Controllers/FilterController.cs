using System.Security.Claims;
using ChatZone.Core.Extensions.Exceptions;
using ChatZone.DTO.Requests;
using ChatZone.DTO.Responses;
using ChatZone.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ChatZone.Controllers;

[ApiController]
[Route("[controller]")]
public class FilterController(IFilterService filterService) : ControllerBase
{
    [Authorize(Roles = "User")]
    [HttpGet]
    [Route("/{username}/filter")]
    public async Task<PersonFilterResponse> GetPersonFilter([FromRoute] string username, CancellationToken cancellationToken)
    {
        var tokenUsername = User.FindFirst(ClaimTypes.Name)?.Value;
        var id = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (tokenUsername is null || id is null) throw new Exception("User does not exist!");
        if (tokenUsername != username) throw new ForbiddenAccessException("You are not an owner!");

        var result = await filterService.GetPersonFilterAsync(int.Parse(id), cancellationToken);
        return result.Match<PersonFilterResponse>(e => e, x=> throw x);
    }

    [Authorize(Roles = "User")]
    [HttpPut]
    [Route("/{username}/filter")]
    public async Task<PersonFilterResponse> UpdatePersonFilter(string username, [FromBody] PersonFilterRequest personFilterRequest, CancellationToken cancellationToken)
    {
        var tokenUsername = User.FindFirst(ClaimTypes.Name)?.Value;
        var id = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (tokenUsername is null || id is null) throw new Exception("User does not exist!");
        if (tokenUsername != username) throw new ForbiddenAccessException("You are not an owner!");

        var result = await filterService.UpdatePersonFilterAsync(int.Parse(id), personFilterRequest, cancellationToken);
        return result.Match<PersonFilterResponse>(e => e, e => throw e);
    }
}