using System.Security.Claims;
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
        var usernameToken = User.FindFirst(ClaimTypes.Name).Value;
        
        var result = await filterService.GetPersonFilterAsync(usernameToken, username);

        return result.Match<PersonFilterResponse>(e => e, x=> throw x);
    }
}