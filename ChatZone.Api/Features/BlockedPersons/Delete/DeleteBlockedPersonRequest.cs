using ChatZone.Shared.DTOs;
using MediatR;

namespace ChatZone.Features.BlockedPersons.Delete;

public class DeleteBlockedPersonRequest : IRequest<Result<bool>>
{
    public int IdPerson { get; init; }
    public int IdBlockedPerson { get; init; }
}