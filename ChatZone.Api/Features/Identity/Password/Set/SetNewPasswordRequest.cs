using System.ComponentModel.DataAnnotations;
using ChatZone.Core.Extensions;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ChatZone.Features.Identity.Password.Set;

public class SetNewPasswordRequest : IRequest<Result<IActionResult>>
{
    public required string Token { get; set; }
    public required string Email { get; set; }
    [MinLength(8, ErrorMessage = "Password can not be less than 8")]
    [MaxLength(50, ErrorMessage = "Password can not be more than 50")] 
    public required string Password { get; set; }
}