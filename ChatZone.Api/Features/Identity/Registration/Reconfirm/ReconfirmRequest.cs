using ChatZone.Core.Extensions;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ChatZone.Features.Identity.Registration.Reconfirm;

public class ReconfirmRequest : IRequest<Result<IActionResult>>
{
    public required string Email { get; set; }
}