using ChatZone.Context;
using ChatZone.Core.Extensions;
using ChatZone.Core.Extensions.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ChatZone.Features.Search.Cancel;

public class CancelHandler(ChatZoneDbContext dbContext) : IRequestHandler<CancelRequest, Result<IActionResult>>
{
    public async Task<Result<IActionResult>> Handle(CancelRequest request, CancellationToken cancellationToken)
    {
        var person = await dbContext.MatchQueues
            .SingleOrDefaultAsync(x => x.IdPerson == request.IdPerson, cancellationToken);
        if(person is null) return Result<IActionResult>.Failure(new NotFoundException("Person is not found!"));
        
        dbContext.Remove(person);
        await dbContext.SaveChangesAsync(cancellationToken);
        return Result<IActionResult>.Ok(new ObjectResult(new {message = "Person has been deleted successfully!"}));
    }
}