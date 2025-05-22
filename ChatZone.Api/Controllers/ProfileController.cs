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
public class ProfileController(IProfileService profileService) : ControllerBase
{
    [Authorize(Roles = "User")]
    [HttpGet]
    [Route("/{username}")]
    public async Task<ProfileResponse> GetProfile(string username)
    {
        var usernameFromToken = User.FindFirst(ClaimTypes.Name)?.Value;
        if (usernameFromToken != username) throw new ForbiddenAccessException("You are not an owner");
        
        var person = await profileService.GetProfileAsync(username);
        return person.Match(x => x, x => throw x);
    }
    
    [Authorize(Roles = "User")]
    [HttpGet]
    [Route("/{username}/blocked-users")]
    public async Task<ActionResult<BlockedPersonResponse[]>> GetBlockedPersons(string username)
    {
        var usernameFromToken = User.FindFirst(ClaimTypes.Name)?.Value;

        if (usernameFromToken is null) throw new Exception("Person is not exists!");
        if (usernameFromToken != username) throw new ForbiddenAccessException("You are not an owner");

        var result = await profileService.GetBlockedPersonsAsync(username);

        return result.Match(x => x, x=>throw x);
    }
    
    [Authorize(Roles = "User")]
    [HttpDelete]
    [Route("/{username}/blocked-users/{idBlockedPerson}")]
    public async Task<IActionResult> DeleteBlockedPerson(string username, [FromRoute] int idBlockedPerson)
    {
        var usernameFromToken = User.FindFirst(ClaimTypes.Name)?.Value;

        if (usernameFromToken is null) throw new Exception("Person is not exists!");
        if (usernameFromToken != username) throw new ForbiddenAccessException("You are not an owner");

        var result = await profileService.DeleteBlockedPersonAsync(username, idBlockedPerson);
        return result.Match<IActionResult>(x=>Ok("Person was deleted successfully!"), x => throw x);
    }

    [Authorize(Roles = "User")]
    [HttpGet]
    [Route("/{username}/quick-messages")]
    public async Task<QuickMessageResponse[]> GetQuickMessages(string username)
    {
        var usernameFromToken = User.FindFirst(ClaimTypes.Name)?.Value;

        if (usernameFromToken is null) throw new Exception("Person is not exists!");
        if (usernameFromToken != username) throw new ForbiddenAccessException("You are not an owner");

        var result = await profileService.GetQuickMessagesAsync(username);

        return result.Match<QuickMessageResponse[]>(x => x, x => throw x);
    }

    [Authorize(Roles = "User")]
    [HttpPost]
    [Route("/{username}/quick-message")]
    public async Task<QuickMessageResponse> CreateQuickMessage(string username,
        [FromBody] QuickMessageRequest quickMessageRequest)
    {
        var usernameFromToken = User.FindFirst(ClaimTypes.Name)?.Value;

        if (usernameFromToken is null) throw new Exception("Person is not exists!");
        if (usernameFromToken != username) throw new ForbiddenAccessException("You are not an owner");

        var result = await profileService.CreateQuickMessagesAsync(username, quickMessageRequest.Message);
        return result.Match<QuickMessageResponse>(x => x, x => throw x);
    }
}