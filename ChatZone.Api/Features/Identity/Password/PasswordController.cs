using System.Security.Claims;
using ChatZone.Features.Identity.Password.Reset;
using ChatZone.Features.Identity.Password.Set;
using ChatZone.Features.Identity.Password.Update;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ChatZone.Features.Identity.Password;

[ApiController]
[Route("[controller]")]
public class PasswordController(IMediator mediator) : ControllerBase
{

    [AllowAnonymous]
    [HttpPost]
    [Route("reset-password")]
    public async Task<IActionResult> ResetPassword([FromQuery] ResetPasswordRequest request, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(request, cancellationToken);
        return result.Match<IActionResult>(_ => Ok(new {message = "The reset link was sent to your email!"}), x => throw x);
    }

    [ValidateAntiForgeryToken]
    [Authorize(Roles = "User")]
    [HttpPut]
    [Route("change-password")]
    public async Task<IActionResult> UpdatePassword([FromBody]UpdatePasswordRequest request, CancellationToken cancellationToken)
    {
        var idPerson = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (idPerson is null) return Unauthorized(new { message = "You are not authorized!" });
        request.IdPerson = int.Parse(idPerson);
        
        var result = await mediator.Send(request, cancellationToken);
        
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
            
            Response.Cookies.Append("XSRF-TOKEN", "", new CookieOptions
            {
                HttpOnly = false,
                Secure = true,
                SameSite = SameSiteMode.None,
                Expires = DateTimeOffset.UtcNow.AddDays(-1)
            });
        }
        
        return result.Match<IActionResult>(_ => Ok(new { message = "Update password has completed successfully!" }), x=> throw x);
    }

    [AllowAnonymous]
    [HttpPut]
    [Route("set-password")]
    public async Task<IActionResult> SetNewPassword([FromBody] SetNewPasswordRequest request, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(request, cancellationToken);
        
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
            
            Response.Cookies.Append("XSRF-TOKEN", "", new CookieOptions
            {
                HttpOnly = false,
                Secure = true,
                SameSite = SameSiteMode.None,
                Expires = DateTimeOffset.UtcNow.AddDays(-1)
            });
        }
        
        return result.Match<IActionResult>(_ => Ok(new { message = "Update password has completed successfully!" }), x=> throw x);
    }
}