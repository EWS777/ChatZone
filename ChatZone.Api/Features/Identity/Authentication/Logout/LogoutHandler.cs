using ChatZone.Core.Extensions.Exceptions;
using ChatZone.Shared.Context;
using ChatZone.Shared.DTOs;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ChatZone.Features.Identity.Authentication.Logout;

public class LogoutHandler(ChatZoneDbContext dbContext) : IRequestHandler<LogoutRequest, Result<bool>>
{
    public async Task<Result<bool>> Handle(LogoutRequest request, CancellationToken cancellationToken)
    {
        var person = await dbContext.Persons.SingleOrDefaultAsync(x => x.IdPerson == request.IdPerson, cancellationToken);
        if(person is null) return Result<bool>.Failure(new NotFoundException("The person does not exist!"));

        person.RefreshToken = "";
        person.RefreshTokenExp = DateTimeOffset.UtcNow.AddDays(-1);

        dbContext.Persons.Update(person);
        await dbContext.SaveChangesAsync(cancellationToken);
        
        return Result<bool>.Ok(true);
    }
}