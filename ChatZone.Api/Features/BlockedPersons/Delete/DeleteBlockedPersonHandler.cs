using ChatZone.Core.Extensions.Exceptions;
using ChatZone.Shared.Context;
using ChatZone.Shared.DTOs;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ChatZone.Features.BlockedPersons.Delete;

public class DeleteBlockedPersonHandler(ChatZoneDbContext dbContext) : IRequestHandler<DeleteBlockedPersonRequest, Result<bool>>
{
    public async Task<Result<bool>> Handle(DeleteBlockedPersonRequest request, CancellationToken cancellationToken)
    {
        var deletePerson = await dbContext.BlockedPeoples.SingleOrDefaultAsync(x => x.IdBlockerPerson == request.IdPerson && x.IdBlockedPerson == request.IdBlockedPerson, cancellationToken);
        if (deletePerson is null) return Result<bool>.Failure(new NotFoundException("Person is not found!"));

        dbContext.BlockedPeoples.Remove(deletePerson);
        await dbContext.SaveChangesAsync(cancellationToken);
        
        return Result<bool>.Ok(true);
    }
}