using ChatZone.Core.Extensions.Exceptions;
using ChatZone.Shared.Context;
using ChatZone.Shared.DTOs;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ChatZone.Features.QuickMessages.Delete;

public class DeleteQuickMessageHandler(ChatZoneDbContext dbContext) : IRequestHandler<DeleteQuickMessageRequest, Result<bool>>
{
    public async Task<Result<bool>> Handle(DeleteQuickMessageRequest request, CancellationToken cancellationToken)
    {
        var message = await dbContext.QuickMessages.SingleOrDefaultAsync(x => x.IdPerson == request.IdPerson && x.IdQuickMessage == request.IdMessage, cancellationToken);
        if(message is null) return Result<bool>.Failure(new NotFoundException("This quick messages doesn't exist!"));
        
        dbContext.QuickMessages.Remove(message);
        await dbContext.SaveChangesAsync(cancellationToken);
        return Result<bool>.Ok(true);
    }
}