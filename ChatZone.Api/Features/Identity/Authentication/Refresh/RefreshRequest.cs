using ChatZone.Shared.DTOs;
using MediatR;

namespace ChatZone.Features.Identity.Authentication.Refresh;

public class RefreshRequest : IRequest<Result<RefreshResponse>>
{
    public required string RefreshToken { get; set; }
}