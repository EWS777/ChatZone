using ChatZone.Core.Extensions;
using MediatR;

namespace ChatZone.Features.Profiles.Update;

public class UpdateProfileRequest : IRequest<Result<UpdateProfileResponse>>
{
    public int Id { get; set; }
    public required string Username { get; set; }
    public bool IsFindByProfile { get; set; }
}