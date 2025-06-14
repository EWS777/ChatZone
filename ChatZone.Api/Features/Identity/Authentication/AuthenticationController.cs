using System.Security.Claims;
using ChatZone.Features.Identity.Authentication.Login;
using ChatZone.Features.Identity.Authentication.Refresh;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ChatZone.Features.Identity.Authentication;

[ApiController]
[Route("[controller]")]
public class AuthenticationController(IMediator mediator) : ControllerBase
{
    [AllowAnonymous]
    [HttpPost]
    [Route("login")]
    public async Task<IActionResult> Login([FromBody]LoginRequest request, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(request, cancellationToken);

        if (!result.IsSuccess) return Unauthorized(result.Exception);
        
        Response.Cookies.Append("AccessToken", result.Value.AccessToken, new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.None,
            Expires = DateTimeOffset.UtcNow.AddDays(7)
        });

        return Ok(new { result.Value.RefreshToken });
    }

    [Authorize(AuthenticationSchemes = "IgnoreTokenExpirationScheme", Roles = "User")]
    [HttpPost]
    [Route("refresh")]
    public async Task<RefreshResponse> Refresh(CancellationToken cancellationToken)
    {
        var id = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (id is null) throw new Exception("User does not exist!");
        
        var result = await mediator.Send(new RefreshRequest{Id = int.Parse(id)}, cancellationToken);
        return result.Match<RefreshResponse>(e => e, x => throw x);
    }
}