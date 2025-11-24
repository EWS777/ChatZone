using ChatZone.Shared.DTOs;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ChatZone.Features.BlockedPersons.Delete;

public class DeleteBlockedPersonRequest : IRequest<Result<IActionResult>>
{
    public int IdPerson { get; init; }
    public int IdBlockedPerson { get; init; }
}