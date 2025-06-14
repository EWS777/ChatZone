using ChatZone.Core.Extensions;
using MediatR;

namespace ChatZone.Features.Identity.Password.Update;

public class UpdatePasswordRequest : IRequest<Result<UpdatePasswordResponse>>
{
    public int Id { get; init; }
    public required string Password { get; init; }
}