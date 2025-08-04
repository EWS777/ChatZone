using ChatZone.Core.Extensions;
using MediatR;

namespace ChatZone.Features.Identity.Authentication.Refresh;

public class RefreshRequest : IRequest<Result<RefreshResponse>>
{
    public int IdPerson { get; set; }
}