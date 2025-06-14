using System.Security.Claims;
using ChatZone.Core.Extensions.Exceptions;
using ChatZone.Features.Identity.Password.Reset;
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
    public async Task<IActionResult> ResetPassword(string email, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new ResetPasswordRequest{Email = email}, cancellationToken);
        return result.Match<IActionResult>(e => Ok("The reset link was sent to your email"), x => throw x);
    }

    [Authorize(AuthenticationSchemes = "IgnoreTokenExpirationScheme", Roles = "User")]
    [HttpPut]
    [Route("change-password")]
    public async Task<UpdatePasswordResponse> UpdatePassword(string username, string password, CancellationToken cancellationToken)
    {
        var tokenUsername = User.FindFirst(ClaimTypes.Name)?.Value;
        var id = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (tokenUsername is null || id is null) throw new Exception("User does not exist!");
        if (tokenUsername != username) throw new ForbiddenAccessException("You are not an owner!");
        
        var result = await mediator.Send(new UpdatePasswordRequest{Id = int.Parse(id), Password = password}, cancellationToken);
        return result.Match<UpdatePasswordResponse>(x=> x, x=> throw x);
    }
}