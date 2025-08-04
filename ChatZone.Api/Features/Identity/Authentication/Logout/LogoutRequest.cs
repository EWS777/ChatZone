using ChatZone.Core.Extensions;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ChatZone.Features.Identity.Authentication.Logout;

public class LogoutRequest : IRequest<Result<IActionResult>>
{
    public int IdPerson { get; init; }
}