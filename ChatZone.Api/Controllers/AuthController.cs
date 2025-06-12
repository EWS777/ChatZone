using System.Security.Claims;
using ChatZone.Core.Extensions.Exceptions;
using ChatZone.DTO.Requests;
using ChatZone.DTO.Responses;
using ChatZone.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ChatZone.Controllers;

[ApiController]
[Route("[controller]")]
public class AuthController(IAuthService authService) : ControllerBase
{
    [AllowAnonymous]
    [HttpPost]
    [Route("/register")]
    public async Task<IActionResult> Register([FromBody]RegisterRequest request)
    {
        var result = await authService.RegisterPersonAsync(request);
        return result.Match<IActionResult>(x=>Ok(new {message = "Completed!"}), x => throw x);
    }
    
    [Authorize(Roles = "Unconfirmed")]
    [HttpPost]
    [Route("/confirm")]
    public async Task<RegisterResponse> Confirm()
    {
        var id = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (id is null) throw new Exception("User does not exist!");

        var result = await authService.ConfirmEmailAsync(int.Parse(id));
        return result.Match<RegisterResponse>(e => e, x => throw x);
    }

    [AllowAnonymous]
    [HttpPost]
    [Route("/login")]
    public async Task<IActionResult> Login([FromBody]LoginRequest request)
    {
        var result = await authService.LoginAsync(request.UsernameOrEmail, request.Password);

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
    [Route("/refresh")]
    public async Task<RegisterResponse> Refresh()
    {
        var id = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (id is null) throw new Exception("User does not exist!");
        
        var result = await authService.RefreshTokenAsync(int.Parse(id));
        return result.Match<RegisterResponse>(e => e, x => throw x);
    }

    [AllowAnonymous]
    [HttpPost]
    [Route("/reset-password")]
    public async Task<IActionResult> ResetPassword(string email)
    {
        var result = await authService.ResetPasswordAsync(email);
        return result.Match<IActionResult>(e => Ok("The reset link was sent to your email"), x => throw x);
    }

    [Authorize(AuthenticationSchemes = "IgnoreTokenExpirationScheme", Roles = "User")]
    [HttpPut]
    [Route("/change-password")]
    public async Task<RegisterResponse> UpdatePassword(string username, string password)
    {
        var tokenUsername = User.FindFirst(ClaimTypes.Name)?.Value;
        var id = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (tokenUsername is null || id is null) throw new Exception("User does not exist!");
        if (tokenUsername != username) throw new ForbiddenAccessException("You are not an owner!");
        
        var result = await authService.UpdatePasswordAsync(int.Parse(id), password);
        return result.Match<RegisterResponse>(e=>e, x=> throw x);
    }
    
    [Authorize(Roles = "User")]
    [HttpPut]
    [Route("/{username}")]
    public async Task<UpdateProfileResponse> UpdateProfile(string username, [FromBody] ProfileRequest profileRequest)
    {
        var tokenUsername = User.FindFirst(ClaimTypes.Name)?.Value;
        var id = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (tokenUsername is null || id is null) throw new Exception("User does not exist!");
        if (tokenUsername != username) throw new ForbiddenAccessException("You are not an owner!");

        var result = await authService.UpdateProfileAsync(int.Parse(id), profileRequest);
        return result.Match(x => x, x => throw x);
    }
}