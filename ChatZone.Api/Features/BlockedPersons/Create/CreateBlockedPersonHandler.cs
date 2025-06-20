using ChatZone.Context;
using ChatZone.Core.Extensions;
using ChatZone.Core.Extensions.Exceptions;
using ChatZone.Core.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ChatZone.Features.BlockedPersons.Create;

public class CreateBlockedPersonHandler(
    ChatZoneDbContext dbContext) : IRequestHandler<CreateBlockedPersonRequest, Result<IActionResult>>
{
    public async Task<Result<IActionResult>> Handle(CreateBlockedPersonRequest request, CancellationToken cancellationToken)
    {
        var person = await dbContext.Persons.AnyAsync(x => x.IdPerson == request.IdPerson, cancellationToken);
        if(!person) return Result<IActionResult>.Failure(new NotFoundException("User is not found!"));

        await dbContext.BlockedPeoples.AddAsync(new BlockedPerson
        {
            IdBlockerPerson = request.IdPerson,
            IdBlockedPerson = request.IdBlockedPerson,
            CreatedAt = DateTime.UtcNow
        }, cancellationToken);

        await dbContext.SaveChangesAsync(cancellationToken);
        return Result<IActionResult>.Ok(new OkObjectResult("User was deleted successfully!"));
    }
}