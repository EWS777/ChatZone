using ChatZone.Core.Extensions;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ChatZone.Features.Identity.Registration.Register;

public class RegisterRequest : IRequest<Result<IActionResult>>
{
    public required string Email { get; set; }
    public required string Username { get; set; }
    public required string Password { get; set; }
}