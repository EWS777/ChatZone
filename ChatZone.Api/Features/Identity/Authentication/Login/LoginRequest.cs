using System.ComponentModel.DataAnnotations;
using ChatZone.Shared.DTOs;
using MediatR;

namespace ChatZone.Features.Identity.Authentication.Login;

public class LoginRequest : IRequest<Result<LoginResponse>>
{
    public required string UsernameOrEmail { get; set; }
    
    [Required(ErrorMessage = "Password is required!")]
    [MinLength(8, ErrorMessage = "Password min length is 8 characters!")]
    [MaxLength(64, ErrorMessage = "Password max length is 64 characters!")]
    public required string Password { get; set; }
}