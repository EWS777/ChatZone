using System.ComponentModel.DataAnnotations;
using ChatZone.Shared.DTOs;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ChatZone.Features.Profiles.Delete;

public class DeleteProfileRequest : IRequest<Result<IActionResult>>
{
    public int IdPerson { get; set; }
    [Required(ErrorMessage = "Password is required!")]
    [MinLength(8, ErrorMessage = "Password min length is 8 characters!")]
    [MaxLength(64, ErrorMessage = "Password max length is 64 characters!")]
    public required string Password { get; set; }
}