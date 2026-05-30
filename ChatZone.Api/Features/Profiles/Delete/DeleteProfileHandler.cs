using ChatZone.Core.Extensions.Exceptions;
using ChatZone.Core.Models.Enums;
using ChatZone.Shared.Context;
using ChatZone.Shared.DTOs;
using ChatZone.Shared.Security;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ChatZone.Features.Profiles.Delete;

public class DeleteProfileHandler(ChatZoneDbContext dbContext, IConfiguration configuration) : IRequestHandler<DeleteProfileRequest, Result<bool>>
{
    public async Task<Result<bool>> Handle(DeleteProfileRequest request, CancellationToken cancellationToken)
    {
        var person = await dbContext.Persons
            .SingleOrDefaultAsync(x => x.IdPerson == request.IdPerson, cancellationToken);
        if(person is null) return Result<bool>.Failure(new NotFoundException("User is not found"));

        var currentHashedPassword = SecurityHelper.GetHashedPasswordWithSalt(request.Password, person.Salt, configuration);
        if(currentHashedPassword != person.Password)
            return Result<bool>.Failure(new ForbiddenAccessException("Password is not correct!"));

        person.Role = PersonRole.Deleted;
        person.Username = $"del_{person.IdPerson}";
        person.Email = $"deleted_{person.IdPerson}@chatzone.local";
        person.Password = "DELETED";
        person.Salt = "";
        person.RefreshToken = "";
        person.RefreshTokenExp = DateTimeOffset.MinValue;
        
        dbContext.Persons.Update(person);
        await dbContext.SaveChangesAsync(cancellationToken);
        
        return Result<bool>.Ok(true);
    }
}