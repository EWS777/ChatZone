using ChatZone.Shared.DTOs;
using MediatR;

namespace ChatZone.Features.Filters.Get;

public class GetFilterRequest : IRequest<Result<GetFilterResponse>>
{
    public int IdPerson { get; set; }
}