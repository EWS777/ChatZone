using ChatZone.Context;
using ChatZone.Core.Extensions;
using ChatZone.Core.Extensions.Exceptions;
using ChatZone.Security;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ChatZone.Features.Identity.Password.Update;

public class UpdatePasswordHandler(ChatZoneDbContext dbContext) : IRequestHandler<UpdatePasswordRequest, Result<IActionResult>>
{
    public async Task<Result<IActionResult>> Handle(UpdatePasswordRequest request, CancellationToken cancellationToken)
    {
        var person = await dbContext.Persons.SingleOrDefaultAsync(x=>x.IdPerson == request.Id, cancellationToken);
        if (person is null) return Result<IActionResult>.Failure(new NotFoundException("User is not found!"));
        
        var oldHashedPassword = SecurityHelper.GetHashedPasswordWithSalt(request.OldPassword, person.Salt);
        if (oldHashedPassword != person.Password)
            return Result<IActionResult>.Failure(new ForbiddenAccessException("Password is not correct!"));
        
        var newHashedPassword = SecurityHelper.GetHashedPasswordWithSalt(request.NewPassword, person.Salt);
        if (newHashedPassword == person.Password) return Result<IActionResult>.Failure(new ForbiddenAccessException("Password can not be the same"));
        
        person.Password = newHashedPassword;
        dbContext.Persons.Update(person);
        await dbContext.SaveChangesAsync(cancellationToken);
        
        return Result<IActionResult>.Ok(new OkObjectResult(new {message = "Update has completed successfully!"}));
    }
}