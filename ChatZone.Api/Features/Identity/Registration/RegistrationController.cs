using System.Security.Claims;
using ChatZone.Features.Identity.Registration.Confirm;
using ChatZone.Features.Identity.Registration.Register;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ChatZone.Features.Identity.Registration;

[ApiController]
[Route("[controller]")]
public class RegistrationController(IMediator mediator) : ControllerBase
{
    [AllowAnonymous]
    [HttpPost]
    [Route("register")]
    public async Task<IActionResult> Register([FromBody]RegisterRequest request, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(request, cancellationToken);
        return result.Match<IActionResult>(x=>Ok(new {message = "Completed!"}), x => throw x);
    }
    
    [Authorize(Roles = "Unconfirmed")]
    [HttpPost]
    [Route("confirm")]
    public async Task<ConfirmResponse> Confirm(CancellationToken cancellationToken)
    {
        var id = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (id is null) throw new Exception("User does not exist!");

        var result = await mediator.Send(new ConfirmRequest{Id = int.Parse(id)}, cancellationToken);

        if (result.IsSuccess)
        {
            Response.Cookies.Append("AccessToken", result.Value.AccessToken, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.None,
                Expires = DateTimeOffset.UtcNow.AddDays(7)
            });
        }
        
        return result.Match<ConfirmResponse>(e => e, x => throw x);
    }
}