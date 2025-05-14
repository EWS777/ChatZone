﻿using System.Security.Claims;
using ChatZone.Core.Extensions.Exceptions;
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

        return result.Match<IActionResult>(e => Ok("Completed!"), x => throw x);
    }
    
    [Authorize(Roles = "Unconfirmed")]
    [HttpPost]
    [Route("/confirm")]
    public async Task<RegisterResponse> Confirm()
    {
        var username = User.FindFirst(ClaimTypes.Name).Value;
        
        var result = await authService.ConfirmEmailAsync(username);

        return result.Match<RegisterResponse>(e => e, x => throw x);
    }

    [AllowAnonymous]
    [HttpPost]
    [Route("/login")]
    public async Task<RegisterResponse> Login([FromBody]LoginRequest request)
    {
        var result = await authService.LoginAsync(request.UsernameOrEmail, request.Password);
        return result.Match<RegisterResponse>(e => e, x => throw x);
    }

    [Authorize(AuthenticationSchemes = "IgnoreTokenExpirationScheme", Roles = "User")]
    [HttpPost]
    [Route("/refresh")]
    public async Task<RegisterResponse> Refresh()
    {
        var username = User.FindFirst(ClaimTypes.Name).Value;
        var result = await authService.RefreshTokenAsync(username);
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
    public async Task<RegisterResponse> UpdatePassword(string password)
    {
        var username = User.FindFirst(ClaimTypes.Name).Value;
        
        var result = await authService.UpdatePasswordAsync(username, password);
        return result.Match<RegisterResponse>(e=>e, x=> throw x);
    }
    
    [Authorize(Roles = "User")]
    [HttpPut]
    [Route("/{username}")]
    public async Task<UpdateProfileResponse> UpdateProfile(string username, [FromBody] ProfileRequest profileRequest)
    {
        var usernameFromToken = User.FindFirst(ClaimTypes.Name)?.Value;

        if (usernameFromToken is null) throw new Exception("Person is not exists!");
        if (usernameFromToken != username) throw new ForbiddenAccessException("You are not an owner");

        var person = await authService.UpdateProfileAsync(usernameFromToken, profileRequest);

        return person.Match(x => x, x => throw x);
    }
}