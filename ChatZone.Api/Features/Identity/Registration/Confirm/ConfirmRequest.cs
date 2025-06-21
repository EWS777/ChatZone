using ChatZone.Core.Extensions;
using MediatR;

namespace ChatZone.Features.Identity.Registration.Confirm;

public class ConfirmRequest : IRequest<Result<ConfirmResponse>>
{
    public required string Token { get; init; }
}