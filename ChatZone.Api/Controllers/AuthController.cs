using ChatZone.Core.Extensions;
using ChatZone.DTO.Requests;
using ChatZone.DTO.Responses;
using ChatZone.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ChatZone.Controllers;

[ApiController]
[Route("[controller]")]
public class AuthController(
    IAuthService authService) : ControllerBase
{
    [AllowAnonymous]
    [HttpPost]
    [Route("/register")]
    public async Task<IActionResult> Register([FromBody]RegisterRequest request)
    {
        var result = await authService.RegisterPersonAsync(request);

        return result.Final<IActionResult>(
            ifSuccess: Ok("Completed!"),
            ifFailure: (status, message, exception) => 
                BadRequest(new { Status = status, Error = message })
        );
    }
    
    [Authorize(Roles = "Unconfirmed")]
    [HttpPost]
    [Route("/confirm")]
    public async Task<RegisterResponse> Confirm()
    {
        var result = await authService.ConfirmEmailAsync(User.Claims);
    }
}