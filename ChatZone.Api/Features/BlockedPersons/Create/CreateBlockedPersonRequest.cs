using ChatZone.Shared.DTOs;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ChatZone.Features.BlockedPersons.Create;

public class CreateBlockedPersonRequest : IRequest<Result<IActionResult>>
{
    public int IdPerson { get; init; }
    public int IdPartnerPerson { get; init; }
}