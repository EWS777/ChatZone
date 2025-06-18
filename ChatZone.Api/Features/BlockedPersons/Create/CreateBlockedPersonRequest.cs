using ChatZone.Core.Extensions;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ChatZone.Features.BlockedPersons.Create;

public class CreateBlockedPersonRequest : IRequest<Result<IActionResult>>
{
    public int IdPerson { get; init; }
    public int IdBlockedPerson { get; init; }
}