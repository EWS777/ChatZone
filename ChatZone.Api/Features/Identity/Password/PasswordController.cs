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
        return result.Match<IActionResult>(x => x, x => throw x);
    }

    [Authorize(Roles = "User")]
    [HttpPut]
    [Route("change-password")]
    public async Task<IActionResult> UpdatePassword([FromBody]UpdatePasswordRequest request, CancellationToken cancellationToken)
    {
        var idPerson = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (idPerson is null) throw new Exception("User does not exist!");
        request.IdPerson = int.Parse(idPerson);
        
        var result = await mediator.Send(request, cancellationToken);
        return result.Match<IActionResult>(x=> x, x=> throw x);
    }

    [AllowAnonymous]
    [HttpPut]
    [Route("set-password")]
    public async Task<IActionResult> SetNewPassword([FromBody] SetNewPasswordRequest request, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(request, cancellationToken);
        
        return result.Match<IActionResult>(x => x, x => throw x);
    }
}