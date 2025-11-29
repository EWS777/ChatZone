using System.ComponentModel.DataAnnotations;
using ChatZone.Shared.DTOs;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ChatZone.Features.Identity.Password.Update;

public class UpdatePasswordRequest : IRequest<Result<IActionResult>>
{
    public int IdPerson { get; set; }
    [Required(ErrorMessage = "Old password is required!")]
    [MinLength(8, ErrorMessage = "Password can not be less than 8")]
    [MaxLength(64, ErrorMessage = "Password can not be more than 64")] 
    public required string OldPassword { get; init; }
    [Required(ErrorMessage = "New password is required!")]
    [MinLength(8, ErrorMessage = "Password can not be less than 8")]
    [MaxLength(64, ErrorMessage = "Password can not be more than 64")] 
    public required string NewPassword { get; init; }
}