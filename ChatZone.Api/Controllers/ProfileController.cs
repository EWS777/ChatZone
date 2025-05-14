using System.Security.Claims;
using ChatZone.Core.Extensions.Exceptions;
using ChatZone.Core.Models;
using ChatZone.DTO.Requests;
using ChatZone.DTO.Responses;
using ChatZone.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ChatZone.Controllers;

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
    public async Task<ActionResult<BlockedPerson[]>> GetBlockedPersons(string username)
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
    public async Task<IActionResult> DeleteBlockedPerson(string username, [FromHeader] int idBlockedPerson)
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
    public async Task<QuickMessage[]> GetQuickMessages(string username)
    {
        var usernameFromToken = User.FindFirst(ClaimTypes.Name)?.Value;

        if (usernameFromToken is null) throw new Exception("Person is not exists!");
        if (usernameFromToken != username) throw new ForbiddenAccessException("You are not an owner");

        var result = await profileService.GetQuickMessagesAsync(username);

        return result.Match<QuickMessage[]>(x => x, x => throw x);
    }

    [Authorize(Roles = "User")]
    [HttpPost]
    [Route("/{username}/quick-massage")]
    public async Task<QuickMessage> CreateQuickMessage(string username,
        [FromBody] QuickMessageRequest quickMessageRequest)
    {
        var usernameFromToken = User.FindFirst(ClaimTypes.Name)?.Value;

        if (usernameFromToken is null) throw new Exception("Person is not exists!");
        if (usernameFromToken != username) throw new ForbiddenAccessException("You are not an owner");

        var result = await profileService.CreateQuickMessagesAsync(username, quickMessageRequest.Message);
        return result.Match<QuickMessage>(x => x, x => throw x);
    }
}