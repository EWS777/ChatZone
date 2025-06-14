using ChatZone.Core.Extensions;
using MediatR;

namespace ChatZone.Features.Profiles.Get;

public class GetProfileRequest : IRequest<Result<GetProfileResponse>>
{
    public int Id { get; init; }
}