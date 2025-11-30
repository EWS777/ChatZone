using System.ComponentModel.DataAnnotations;
using ChatZone.Shared.DTOs;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ChatZone.Features.Identity.Registration.Reconfirm;

public class ReconfirmRequest : IRequest<Result<IActionResult>>
{
    [Required(ErrorMessage = "Email is required!")]
    [EmailAddress(ErrorMessage = "Invalid email format!")]
    [MinLength(5, ErrorMessage = "Email min length is 5 characters!")]
    [MaxLength(254, ErrorMessage = "Email max length is 254 characters!")]
    public required string Email { get; set; }
}