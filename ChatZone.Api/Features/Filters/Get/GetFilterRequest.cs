using ChatZone.Core.Extensions;
using MediatR;

namespace ChatZone.Features.Filters.Get;

public class GetFilterRequest : IRequest<Result<GetFilterResponse>>
{
    public int Id { get; set; }
}