using ChatZone.Context;
using ChatZone.Core.Extensions;
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
        var idBlockedPerson = await dbContext.Persons
            .Where(x => x.Username == request.UsernameBlockedPerson)
            .Select(x => x.IdPerson)
            .SingleOrDefaultAsync(cancellationToken);
        
        await dbContext.BlockedPeoples.AddAsync(new BlockedPerson
        {
            IdBlockerPerson = request.IdPerson,
            IdBlockedPerson = idBlockedPerson,
            CreatedAt = DateTimeOffset.UtcNow
        }, cancellationToken);

        await dbContext.SaveChangesAsync(cancellationToken);
        return Result<IActionResult>.Ok(new OkObjectResult("Person has blocked successfully!"));
    }
}