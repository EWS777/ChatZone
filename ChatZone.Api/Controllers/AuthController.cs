using ChatZone.Core.DTO.Requests;
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
    public async Task<IActionResult> Register(RegisterRequest request)
    {
        var result = await authService.RegisterPersonAsync(request);

        return result.Final<IActionResult>(
            ifSuccess: Ok("Completed!"),
            ifFailure: (status, message, exception) => 
                BadRequest(new { Status = status, Error = message })
        );
    }
}