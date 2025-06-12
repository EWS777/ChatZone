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
    public async Task<ProfileResponse> GetProfile(string username, CancellationToken cancellationToken)
    {
        var tokenUsername = User.FindFirst(ClaimTypes.Name)?.Value;
        var id = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (tokenUsername is null || id is null) throw new Exception("User does not exist!");
        if (tokenUsername != username) throw new ForbiddenAccessException("You are not an owner!");

        var result = await profileService.GetProfileAsync(int.Parse(id), cancellationToken);
        return result.Match(x => x, x => throw x);
    }
    
    [Authorize(Roles = "User")]
    [HttpGet]
    [Route("/{username}/blocked-users")]
    public async Task<List<BlockedPersonResponse>> GetBlockedPersons(string username, CancellationToken cancellationToken)
    {
        var tokenUsername = User.FindFirst(ClaimTypes.Name)?.Value;
        var id = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (tokenUsername is null || id is null) throw new Exception("User does not exist!");
        if (tokenUsername != username) throw new ForbiddenAccessException("You are not an owner!");

        var result = await profileService.GetBlockedPersonsAsync(int.Parse(id), cancellationToken);
        return result.Match(x => x, x=>throw x);
    }

    [Authorize(Roles = "User")]
    [HttpGet]
    [Route("/{username}/quick-messages")]
    public async Task<List<QuickMessageResponse>> GetQuickMessages(string username, CancellationToken cancellationToken)
    {
        var tokenUsername = User.FindFirst(ClaimTypes.Name)?.Value;
        var id = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (tokenUsername is null || id is null) throw new Exception("User does not exist!");
        if (tokenUsername != username) throw new ForbiddenAccessException("You are not an owner!");

        var result = await profileService.GetQuickMessagesAsync(int.Parse(id), cancellationToken);
        return result.Match(x => x, x => throw x);
    }

    [Authorize(Roles = "User")]
    [HttpPost]
    [Route("/{username}/quick-message")]
    public async Task<QuickMessageResponse> AddQuickMessage(string username, [FromBody] QuickMessageRequest quickMessageRequest, CancellationToken cancellationToken)
    {
        var tokenUsername = User.FindFirst(ClaimTypes.Name)?.Value;
        var id = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (tokenUsername is null || id is null) throw new Exception("User does not exist!");
        if (tokenUsername != username) throw new ForbiddenAccessException("You are not an owner!");

        var result = await profileService.AddQuickMessageAsync(int.Parse(id), quickMessageRequest.Message, cancellationToken);
        return result.Match<QuickMessageResponse>(x => x, x => throw x);
    }

    [Authorize(Roles = "User")]
    [HttpDelete]
    [Route("/{username}/blocked-users/{idBlockedPerson}")]
    public async Task<IActionResult> DeleteBlockedPerson(string username, [FromRoute] int idBlockedPerson, CancellationToken cancellationToken)
    {
        var tokenUsername = User.FindFirst(ClaimTypes.Name)?.Value;
        var id = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (tokenUsername is null || id is null) throw new Exception("User does not exist!");
        if (tokenUsername != username) throw new ForbiddenAccessException("You are not an owner!");

        var result = await profileService.DeleteBlockedPersonAsync(int.Parse(id), idBlockedPerson, cancellationToken);
        return result.Match<IActionResult>(x=>Ok("Person was deleted successfully!"), x => throw x);
    }
}