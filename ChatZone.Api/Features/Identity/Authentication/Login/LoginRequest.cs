using ChatZone.Core.Extensions;
using MediatR;

namespace ChatZone.Features.Identity.Authentication.Login;

public class LoginRequest : IRequest<Result<LoginResponse>>
{
    public required string UsernameOrEmail { get; set; }
    public required string Password { get; set; }
}