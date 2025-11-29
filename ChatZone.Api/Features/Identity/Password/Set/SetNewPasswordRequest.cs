using System.ComponentModel.DataAnnotations;
using ChatZone.Shared.DTOs;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ChatZone.Features.Identity.Password.Set;

public class SetNewPasswordRequest : IRequest<Result<IActionResult>>
{
    public required string Token { get; set; }
    [Required(ErrorMessage = "Email is required!")]
    [EmailAddress(ErrorMessage = "Invalid email format!")]
    [MinLength(5, ErrorMessage = "Email min length is 5 characters!")]
    [MaxLength(254, ErrorMessage = "Email max length is 254 characters!")]
    public required string Email { get; set; }
    [Required(ErrorMessage = "Password is required!")]
    [MinLength(8, ErrorMessage = "Password min length is 8 characters!")]
    [MaxLength(64, ErrorMessage = "Password max length is 64 characters!")]
    public required string Password { get; set; }
}