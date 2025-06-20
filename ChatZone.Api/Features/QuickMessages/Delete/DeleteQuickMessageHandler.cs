using ChatZone.Context;
using ChatZone.Core.Extensions;
using ChatZone.Core.Extensions.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ChatZone.Features.QuickMessages.Delete;

public class DeleteQuickMessageHandler(
    ChatZoneDbContext dbContext) : IRequestHandler<DeleteQuickMessageRequest, Result<IActionResult>>
{
    public async Task<Result<IActionResult>> Handle(DeleteQuickMessageRequest request, CancellationToken cancellationToken)
    {
        var person = await dbContext.Persons.AnyAsync(x => x.IdPerson == request.IdPerson, cancellationToken);
        if (!person) return Result<IActionResult>.Failure(new NotFoundException("User is not found!"));
        
        var message = await dbContext.QuickMessages.SingleOrDefaultAsync(x => x.IdQuickMessage == request.IdMessage,
                cancellationToken);
        if(message is null) return Result<IActionResult>.Failure(new NotFoundException("Quick message is not found!"));
        
        dbContext.QuickMessages.Remove(message);
        await dbContext.SaveChangesAsync(cancellationToken);
        return Result<IActionResult>.Ok(new OkObjectResult("Quick message was deleted successfully!"));
    }
}