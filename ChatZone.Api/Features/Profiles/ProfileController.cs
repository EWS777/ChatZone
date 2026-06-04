using System.Security.Claims;
using Microsoft.AspNetCore.Antiforgery;
using ChatZone.Features.Profiles.Delete;
using ChatZone.Features.Profiles.Get;
using ChatZone.Features.Profiles.Update;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ChatZone.Features.Profiles;

[ApiController]
[Route("[controller]")]
public class ProfileController(IMediator mediator,
    IConfiguration configuration,
    IAntiforgery antiforgery) : ControllerBase
{
    [Authorize(Roles = "User")]
    [HttpGet]
    [Route("")]
    public async Task<ActionResult<GetProfileResponse>> GetProfile(CancellationToken cancellationToken)
    {
        var idPerson = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (idPerson is null) return Unauthorized(new { message = "You are not authorized!" });

        var result = await mediator.Send(new GetProfileRequest{IdPerson = int.Parse(idPerson)}, cancellationToken);
        return result.Match(x => x, x => throw x);
    }
    
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "User")]
    [HttpPut]
    [Route("{username}")]
    public async Task<ActionResult<UpdateProfileResponse>> UpdateProfile(string username, [FromBody] UpdateProfileRequest profileRequest, CancellationToken cancellationToken)
    {
        var tokenUsername = User.FindFirst(ClaimTypes.Name)?.Value;
        var idPerson = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (tokenUsername is null || idPerson is null) return Unauthorized(new { message = "You are not authorized!" });
        if (tokenUsername != username) return StatusCode(StatusCodes.Status403Forbidden, new { message = "You are not an owner!" });

        profileRequest.IdPerson = int.Parse(idPerson);
        
        var result = await mediator.Send(profileRequest, cancellationToken);
        
        if (result.IsSuccess && username != result.Value.Username)
        {
            Response.Cookies.Append("AccessToken", result.Value.AccessToken!, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.None,
                Expires = DateTimeOffset.UtcNow.AddMinutes(double.Parse(configuration["JWT:AccessTokenExpMinutes"]!))
            });
            Response.Cookies.Append("RefreshToken", result.Value.RefreshToken!, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.None,
                Expires = DateTimeOffset.UtcNow.AddDays(double.Parse(configuration["JWT:RefreshTokenExpDays"]!))
            });
            
            var tokens = antiforgery.GetAndStoreTokens(HttpContext);
            Response.Cookies.Append("XSRF-TOKEN", tokens.RequestToken!, new CookieOptions
            {
                HttpOnly = false,
                Secure = true,
                SameSite = SameSiteMode.None
            });
        }
        
        return result.Match(x => x, x => throw x);
    }
    
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "User")]
    [HttpPost]
    [Route("delete")]
    public async Task<IActionResult> DeleteProfile([FromBody] DeleteProfileRequest profileRequest,
        CancellationToken cancellationToken)
    {
        var idPerson = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (idPerson is null) return Unauthorized(new { message = "You are not authorized!" });

        profileRequest.IdPerson = int.Parse(idPerson);

        var result = await mediator.Send(profileRequest, cancellationToken);

        if (result.IsSuccess)
        {
            Response.Cookies.Delete("AccessToken", new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.None,
                Expires = DateTimeOffset.UtcNow.AddDays(-1)
            });
            Response.Cookies.Delete("RefreshToken", new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.None,
                Expires = DateTimeOffset.UtcNow.AddDays(-1)
            });
            
            Response.Cookies.Delete("XSRF-TOKEN", new CookieOptions
            {
                HttpOnly = false,
                Secure = true,
                SameSite = SameSiteMode.None,
                Expires = DateTimeOffset.UtcNow.AddDays(-1)
            });
        }

        return result.Match(_ => Ok(new {message = "Profile was deleted successfully!"}), x => throw x);
    }
}