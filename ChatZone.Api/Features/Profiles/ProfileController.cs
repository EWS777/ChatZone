using System.Security.Claims;
using ChatZone.Core.Extensions.Exceptions;
using ChatZone.Features.Profiles.Get;
using ChatZone.Features.Profiles.Update;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ChatZone.Features.Profiles;

[ApiController]
[Route("[controller]")]
public class ProfileController(IMediator mediator) : ControllerBase
{
    [Authorize(Roles = "User")]
    [HttpGet]
    [Route("")]
    public async Task<GetProfileResponse> GetProfile(CancellationToken cancellationToken)
    {
        var id = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (id is null) throw new Exception("User does not exist!");

        var result = await mediator.Send(new GetProfileRequest{Id = int.Parse(id)}, cancellationToken);
        return result.Match(x => x, x => throw x);
    }
    
    [Authorize(Roles = "User")]
    [HttpPut]
    [Route("{username}")]
    public async Task<UpdateProfileResponse> UpdateProfile(string username, [FromBody] UpdateProfileRequest profileRequest, CancellationToken cancellationToken)
    {
        var tokenUsername = User.FindFirst(ClaimTypes.Name)?.Value;
        var id = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (tokenUsername is null || id is null) throw new Exception("User does not exist!");
        if (tokenUsername != username) throw new ForbiddenAccessException("You are not an owner!");

        profileRequest.Id = int.Parse(id);
        
        var result = await mediator.Send(profileRequest, cancellationToken);
        return result.Match(x => x, x => throw x);
    }
}