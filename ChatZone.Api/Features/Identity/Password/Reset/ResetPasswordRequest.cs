using ChatZone.Core.Extensions;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ChatZone.Features.Identity.Password.Reset;

public class ResetPasswordRequest : IRequest<Result<IActionResult>>
{
    public required string Email { get; set; }
}