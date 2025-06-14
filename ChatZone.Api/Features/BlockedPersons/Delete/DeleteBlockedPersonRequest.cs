using ChatZone.Core.Extensions;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ChatZone.Features.BlockedPersons.Delete;

public class DeleteBlockedPersonRequest : IRequest<Result<IActionResult>>
{
    public int Id { get; set; }
    public int IdBlockedPerson { get; set; }
}