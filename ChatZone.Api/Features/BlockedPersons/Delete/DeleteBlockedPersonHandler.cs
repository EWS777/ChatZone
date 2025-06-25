using ChatZone.Context;
using ChatZone.Core.Extensions;
using ChatZone.Core.Extensions.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ChatZone.Features.BlockedPersons.Delete;

public class DeleteBlockedPersonHandler(ChatZoneDbContext dbContext) : IRequestHandler<DeleteBlockedPersonRequest, Result<IActionResult>>
{
    public async Task<Result<IActionResult>> Handle(DeleteBlockedPersonRequest request, CancellationToken cancellationToken)
    {
        var deletePerson = await dbContext.BlockedPeoples.SingleOrDefaultAsync(x => x.IdBlockerPerson == request.Id && x.IdBlockedPerson == request.IdBlockedPerson, cancellationToken);
        if (deletePerson is null) return Result<IActionResult>.Failure(new NotFoundException("Person is not found!"));

        dbContext.BlockedPeoples.Remove(deletePerson);
        await dbContext.SaveChangesAsync(cancellationToken);
        
        return Result<IActionResult>.Ok(new OkObjectResult(new {message = "User was deleted successfully!"}));
    }
}