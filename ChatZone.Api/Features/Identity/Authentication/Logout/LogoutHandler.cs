using ChatZone.Context;
using ChatZone.Core.Extensions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ChatZone.Features.Identity.Authentication.Logout;

public class LogoutHandler(ChatZoneDbContext dbContext) : IRequestHandler<LogoutRequest, Result<IActionResult>>
{
    public async Task<Result<IActionResult>> Handle(LogoutRequest request, CancellationToken cancellationToken)
    {
        var person = await dbContext.Persons.SingleOrDefaultAsync(x => x.IdPerson == request.IdPerson, cancellationToken);

        person!.RefreshToken = "";
        person.RefreshTokenExp = DateTimeOffset.UtcNow.AddDays(-1);

        dbContext.Persons.Update(person);
        await dbContext.SaveChangesAsync(cancellationToken);
        
        return Result<IActionResult>.Ok(new OkObjectResult("Logout has completed!"));
    }
}