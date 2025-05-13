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
    public async Task<PersonFilterResponse> GetPersonFilter([FromRoute] string username)
    {
        var usernameFromToken = User.FindFirst(ClaimTypes.Name)?.Value;
        if (usernameFromToken is null) throw new Exception("Person is not exists!");
        if (usernameFromToken != username) throw new ForbiddenAccessException("You are not an owner");
        
        var result = await filterService.GetPersonFilterAsync(username);

        return result.Match<PersonFilterResponse>(e => e, x=> throw x);
    }

    [Authorize(Roles = "User")]
    [HttpPut]
    [Route("/{username}/filter")]
    public async Task<PersonFilterResponse> UpdatePersonFilter(string username, [FromBody] PersonFilterRequest personFilterRequest)
    {
        var usernameFromToken = User.FindFirst(ClaimTypes.Name)?.Value;
        if (usernameFromToken is null) throw new Exception("Person is not exists!");
        if (usernameFromToken != username) throw new ForbiddenAccessException("You are not an owner");

        var result = await filterService.UpdatePersonFilterAsync(username, personFilterRequest);

        return result.Match<PersonFilterResponse>(e => e, e => throw e);
    }
}