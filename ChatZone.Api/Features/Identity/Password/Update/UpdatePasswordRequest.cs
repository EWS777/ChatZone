using System.ComponentModel.DataAnnotations;
using ChatZone.Core.Extensions;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ChatZone.Features.Identity.Password.Update;

public class UpdatePasswordRequest : IRequest<Result<IActionResult>>
{
    public int IdPerson { get; set; }
    [MinLength(8, ErrorMessage = "Password can not be less than 8")]
    [MaxLength(50, ErrorMessage = "Password can not be more than 50")] 
    public required string OldPassword { get; init; }
    [MinLength(8, ErrorMessage = "Password can not be less than 8")]
    [MaxLength(50, ErrorMessage = "Password can not be more than 50")] 
    public required string NewPassword { get; init; }
}