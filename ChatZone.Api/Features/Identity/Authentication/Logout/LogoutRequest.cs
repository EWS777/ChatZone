using ChatZone.Shared.DTOs;
using MediatR;

namespace ChatZone.Features.Identity.Authentication.Logout;

public class LogoutRequest : IRequest<Result<bool>>
{
    public int IdPerson { get; init; }
}