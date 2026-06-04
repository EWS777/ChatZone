using ChatZone.Features.Identity.Registration.Confirm;
using ChatZone.Features.Identity.Registration.Reconfirm;
using ChatZone.Features.Identity.Registration.Register;
using MediatR;
using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ChatZone.Features.Identity.Registration;

[ApiController]
[Route("[controller]")]
public class RegistrationController(IMediator mediator,
    IConfiguration configuration,
    IAntiforgery antiforgery) : ControllerBase
{
    [ValidateAntiForgeryToken]
    [AllowAnonymous]
    [HttpPost]
    [Route("register")]
    public async Task<IActionResult> Register([FromBody]RegisterRequest request, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(request, cancellationToken);
        return result.Match<IActionResult>(_=>Ok(new {message = "Completed!"}), x => throw x);
    }
    
    [ValidateAntiForgeryToken]
    [AllowAnonymous]
    [HttpPost]
    [Route("confirm")]
    public async Task<ConfirmResponse> Confirm([FromQuery]string token, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new ConfirmRequest{Token = token}, cancellationToken);

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
            
            var tokens = antiforgery.GetAndStoreTokens(HttpContext);
            Response.Cookies.Append("XSRF-TOKEN", tokens.RequestToken!, new CookieOptions
            {
                HttpOnly = false,
                Secure = true,
                SameSite = SameSiteMode.None
            });
        }
        
        return result.Match<ConfirmResponse>(x => x, x => throw x);
    }

    [ValidateAntiForgeryToken]
    [AllowAnonymous]
    [HttpPost]
    [Route("reconfirm")]
    public async Task<IActionResult> Reconfirm([FromQuery] string email, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new ReconfirmRequest{Email = email}, cancellationToken);
        return result.Match<IActionResult>(_ => Ok(new { message = "Link was sent!" }), x => throw x);
    }
}