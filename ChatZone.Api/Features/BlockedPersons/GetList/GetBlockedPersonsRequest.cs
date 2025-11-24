using ChatZone.Shared.DTOs;
using MediatR;

namespace ChatZone.Features.BlockedPersons.GetList;

public class GetBlockedPersonsRequest : IRequest<Result<List<GetBlockedPersonsResponse>>>
{
    public int IdPerson { get; init; }
}