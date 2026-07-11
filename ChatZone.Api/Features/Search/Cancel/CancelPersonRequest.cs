using ChatZone.Shared.DTOs;
using MediatR;

namespace ChatZone.Features.Search.Cancel;

public class CancelPersonRequest : IRequest<Result<bool>>
{
    public int IdPerson { get; init; }
}