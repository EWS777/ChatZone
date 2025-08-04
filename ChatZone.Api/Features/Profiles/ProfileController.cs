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
        var idPerson = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (idPerson is null) throw new Exception("User does not exist!");

        var result = await mediator.Send(new GetProfileRequest{IdPerson = int.Parse(idPerson)}, cancellationToken);
        return result.Match(x => x, x => throw x);
    }
    
    [Authorize(Roles = "User")]
    [HttpPut]
    [Route("{username}")]
    public async Task<UpdateProfileResponse> UpdateProfile(string username, [FromBody] UpdateProfileRequest profileRequest, CancellationToken cancellationToken)
    {
        var tokenUsername = User.FindFirst(ClaimTypes.Name)?.Value;
        var idPerson = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (tokenUsername is null || idPerson is null) throw new Exception("User does not exist!");
        if (tokenUsername != username) throw new ForbiddenAccessException("You are not an owner!");

        profileRequest.IdPerson = int.Parse(idPerson);
        
        var result = await mediator.Send(profileRequest, cancellationToken);
        
        if (result.IsSuccess && username != result.Value.Username)
        {
            Response.Cookies.Append("AccessToken", result.Value.AccessToken!, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.None,
                Expires = DateTimeOffset.UtcNow.AddDays(7)
            });
            Response.Cookies.Append("RefreshToken", result.Value.RefreshToken!, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.None,
                Expires = DateTimeOffset.UtcNow.AddDays(7)
            });
        }
        
        return result.Match(x => x, x => throw x);
    }
}