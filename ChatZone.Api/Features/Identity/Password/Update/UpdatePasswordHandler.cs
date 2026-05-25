using ChatZone.Core.Extensions.Exceptions;
using ChatZone.Shared.Context;
using ChatZone.Shared.DTOs;
using ChatZone.Shared.Security;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ChatZone.Features.Identity.Password.Update;

public class UpdatePasswordHandler(ChatZoneDbContext dbContext, IConfiguration configuration) : IRequestHandler<UpdatePasswordRequest, Result<bool>>
{
    public async Task<Result<bool>> Handle(UpdatePasswordRequest request, CancellationToken cancellationToken)
    {
        var person = await dbContext.Persons.SingleOrDefaultAsync(x=>x.IdPerson == request.IdPerson, cancellationToken);
        if (person is null) return Result<bool>.Failure(new NotFoundException("User is not found!"));
        
        var oldHashedPassword = SecurityHelper.GetHashedPasswordWithSalt(request.OldPassword, person.Salt, configuration);
        if (oldHashedPassword != person.Password)
            return Result<bool>.Failure(new ForbiddenAccessException("Password is not correct!"));
        
        if (request.OldPassword == request.NewPassword) return Result<bool>.Failure(new ForbiddenAccessException("Password can not be the same"));
        
        var newHashedPassword = SecurityHelper.GetHashedPasswordAndSalt(request.NewPassword, configuration);
        
        person.Password = newHashedPassword.Item1;
        person.Salt = newHashedPassword.Item2;
        person.RefreshToken = "";
        person.RefreshTokenExp = DateTimeOffset.UtcNow.AddDays(-1);
        
        dbContext.Persons.Update(person);
        await dbContext.SaveChangesAsync(cancellationToken);
        
        return Result<bool>.Ok(true);
    }
}