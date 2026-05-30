using ChatZone.Core.Extensions.Exceptions;
using ChatZone.Shared.Context;
using ChatZone.Shared.DTOs;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ChatZone.Features.Search.Cancel;

public class CancelPersonHandler(ChatZoneDbContext dbContext) : IRequestHandler<CancelPersonRequest, Result<bool>>
{
    public async Task<Result<bool>> Handle(CancelPersonRequest personRequest, CancellationToken cancellationToken)
    {
        var person = await dbContext.MatchQueues
            .SingleOrDefaultAsync(x => x.IdPerson == personRequest.IdPerson, cancellationToken);
        if(person is null) return Result<bool>.Failure(new NotFoundException("Person is not found!"));
        
        dbContext.Remove(person);
        await dbContext.SaveChangesAsync(cancellationToken);
        return Result<bool>.Ok(true);
    }
}