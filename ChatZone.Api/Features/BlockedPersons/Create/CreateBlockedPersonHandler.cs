using ChatZone.Core.Models;
using ChatZone.Shared.Context;
using ChatZone.Shared.DTOs;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ChatZone.Features.BlockedPersons.Create;

public class CreateBlockedPersonHandler(
    ChatZoneDbContext dbContext) : IRequestHandler<CreateBlockedPersonRequest, Result<bool>>
{
    public async Task<Result<bool>> Handle(CreateBlockedPersonRequest request, CancellationToken cancellationToken)
    {
        var isCurrentPersonBlocked = await dbContext.BlockedPeoples.AnyAsync(x => x.IdBlockerPerson == request.IdPerson && x.IdBlockedPerson == request.IdPartnerPerson, cancellationToken);
        if(isCurrentPersonBlocked) return Result<bool>.Ok(true);
        
        await dbContext.BlockedPeoples.AddAsync(new BlockedPerson
        {
            IdBlockerPerson = request.IdPerson,
            IdBlockedPerson = request.IdPartnerPerson,
            CreatedAt = DateTimeOffset.UtcNow
        }, cancellationToken);

        await dbContext.SaveChangesAsync(cancellationToken);
        return Result<bool>.Ok(true);
    }
}