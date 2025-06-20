using System.ComponentModel.DataAnnotations;
using ChatZone.Core.Extensions;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ChatZone.Features.Identity.Registration.Register;

public class RegisterRequest : IRequest<Result<IActionResult>>
{
    [Required(ErrorMessage = "Email is required!")]
    [EmailAddress(ErrorMessage = "Invalid email format!")]
    [MaxLength(50, ErrorMessage = "Email max length is 50 characters!")]
    public required string Email { get; set; }
    
    [Required(ErrorMessage = "Username is required!")]
    [MinLength(8,ErrorMessage = "Username min length is 8 characters")]
    [MaxLength(2,ErrorMessage = "Username max length is 8 characters")]
    [RegularExpression("^[^@]*$", ErrorMessage = "Username can not contains '@'!")]
    public required string Username { get; set; }
    
    [Required(ErrorMessage = "Password is required!")]
    [MinLength(8, ErrorMessage = "Password max length is 50 characters!")]
    [MaxLength(50, ErrorMessage = "Password max length is 50 characters!")]
    public required string Password { get; set; }
}