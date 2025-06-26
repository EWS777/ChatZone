using ChatZone.Context;
using ChatZone.Core.Extensions;
using ChatZone.Core.Extensions.Exceptions;
using ChatZone.Security;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ChatZone.Features.Identity.Password.Set;

public class SetNewPasswordHandler(
    ChatZoneDbContext dbContext) : IRequestHandler<SetNewPasswordRequest, Result<IActionResult>>
{
    public async Task<Result<IActionResult>> Handle(SetNewPasswordRequest request, CancellationToken cancellationToken)
    {
        var person = await dbContext.Persons.SingleOrDefaultAsync(
                x => x.Email == request.Email && x.EmailConfirmToken == request.Token, cancellationToken);
        
        if(person is null) return Result<IActionResult>.Failure(new NotFoundException("User is not found!"));
        
        var hashedPassword = SecurityHelper.GetHashedPasswordAndSalt(request.Password);

        person.Password = hashedPassword.Item1;
        person.Salt = hashedPassword.Item2;
        
        dbContext.Persons.Update(person);
        await dbContext.SaveChangesAsync(cancellationToken);
        
        return Result<IActionResult>.Ok(new OkObjectResult(new {message = "Update password has completed successfully!"}));
    }
}