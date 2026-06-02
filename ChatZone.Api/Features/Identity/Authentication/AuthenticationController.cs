using System.Security.Claims;
using ChatZone.Core.Extensions.Exceptions;
using ChatZone.Features.Identity.Authentication.Login;
using ChatZone.Features.Identity.Authentication.Logout;
using ChatZone.Features.Identity.Authentication.Refresh;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ChatZone.Features.Identity.Authentication;

[ApiController]
[Route("[controller]")]
public class AuthenticationController(IMediator mediator,
    IConfiguration configuration) : ControllerBase
{
    [Authorize(Roles = "User")]
    [HttpGet]
    [Route("me")]
    public IActionResult Me()
    {
        var username = User.FindFirst(ClaimTypes.Name)?.Value;
        return Ok(new {username});
    }
    
    [AllowAnonymous]
    [HttpPost]
    [Route("login")]
    public async Task<LoginResponse> Login([FromBody]LoginRequest request, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(request, cancellationToken);

        if (result.IsSuccess)
        {
            Response.Cookies.Append("AccessToken", result.Value.AccessToken, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.None,
                Expires = DateTimeOffset.UtcNow.AddMinutes(double.Parse(configuration["JWT:AccessTokenExpMinutes"]!))
            });
            Response.Cookies.Append("RefreshToken", result.Value.RefreshToken, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.None,
                Expires = DateTimeOffset.UtcNow.AddDays(double.Parse(configuration["JWT:RefreshTokenExpDays"]!))
            });
        }
        return result.Match<LoginResponse>(x => x, x => throw x);
    }

    [Authorize(AuthenticationSchemes = "IgnoreTokenExpirationScheme", Roles = "User")]
    [HttpPost]
    [Route("refresh")]
    public async Task<ActionResult<RefreshResponse>> Refresh(CancellationToken cancellationToken)
    {
        var oldRefreshToken = Request.Cookies["RefreshToken"];
        if (string.IsNullOrEmpty(oldRefreshToken)) return UnprocessableEntity(new {message = "Refresh token is missing in cookies!" });
        
        var idPerson = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (idPerson is null) return Unauthorized(new { message = "You are not authorized!" });
        
        var result = await mediator.Send(new RefreshRequest
        {
            IdPerson = int.Parse(idPerson),
            RefreshToken = oldRefreshToken
        }, cancellationToken);
        
        if (result.IsSuccess)
        {
            Response.Cookies.Append("AccessToken", result.Value.AccessToken, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.None,
                Expires = DateTimeOffset.UtcNow.AddMinutes(double.Parse(configuration["JWT:AccessTokenExpMinutes"]!))
            });
            Response.Cookies.Append("RefreshToken", result.Value.RefreshToken, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.None,
                Expires = DateTimeOffset.UtcNow.AddDays(double.Parse(configuration["JWT:RefreshTokenExpDays"]!))
            });
        }
        
        return result.Match<RefreshResponse>(e => e, x => throw x);
    }

    [Authorize(Roles = "User")]
    [HttpPost]
    [Route("logout")]
    public async Task<IActionResult> Logout(CancellationToken cancellationToken)
    {
        var idPerson = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (idPerson is null) return Unauthorized(new { message = "You are not authorized!" });
        
        var result = await mediator.Send(new LogoutRequest{IdPerson = int.Parse(idPerson)}, cancellationToken);
        if (result.IsSuccess)
        {
            Response.Cookies.Append("AccessToken", "", new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.None,
                Expires = DateTimeOffset.UtcNow.AddDays(-1)
            });
        
            Response.Cookies.Append("RefreshToken", "", new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.None,
                Expires = DateTimeOffset.UtcNow.AddDays(-1)
            });
        } 
        return result.Match<IActionResult>(x => Ok(new {message = "Logout has completed successfully!"}), x => throw x);
    }
}