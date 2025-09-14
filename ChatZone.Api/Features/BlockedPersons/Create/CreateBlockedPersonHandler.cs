using ChatZone.Context;
using ChatZone.Core.Extensions;
using ChatZone.Core.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ChatZone.Features.BlockedPersons.Create;

public class CreateBlockedPersonHandler(
    ChatZoneDbContext dbContext) : IRequestHandler<CreateBlockedPersonRequest, Result<IActionResult>>
{
    public async Task<Result<IActionResult>> Handle(CreateBlockedPersonRequest request, CancellationToken cancellationToken)
    {
        await dbContext.BlockedPeoples.AddAsync(new BlockedPerson
        {
            IdBlockerPerson = request.IdPerson,
            IdBlockedPerson = request.IdPartnerPerson,
            CreatedAt = DateTimeOffset.UtcNow
        }, cancellationToken);

        await dbContext.SaveChangesAsync(cancellationToken);
        return Result<IActionResult>.Ok(new OkObjectResult(new {message = "Person has blocked successfully!"}));
    }
}