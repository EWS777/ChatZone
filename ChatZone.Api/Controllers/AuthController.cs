using ChatZone.Core.DTO.Requests;
using ChatZone.Core.DTO.Responses;
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
    public async Task<ActionResult<RegisterResponse>> Register(RegisterRequest request)
    {
        var result = await authService.RegisterPersonAsync(request);
        return result;
    }
}