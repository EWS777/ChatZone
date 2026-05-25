using ChatZone.Core.Extensions.Exceptions;
using ChatZone.Shared.Context;
using ChatZone.Shared.DTOs;
using ChatZone.Shared.Security;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ChatZone.Features.Identity.Password.Set;

public class SetNewPasswordHandler(ChatZoneDbContext dbContext, IConfiguration configuration) : IRequestHandler<SetNewPasswordRequest, Result<bool>>
{
    public async Task<Result<bool>> Handle(SetNewPasswordRequest request, CancellationToken cancellationToken)
    {
        var person = await dbContext.Persons.SingleOrDefaultAsync(x => x.Email == request.Email, cancellationToken);
        if(person is null) return Result<bool>.Failure(new NotFoundException("User is not found!"));
        
        var hashedPasswordResetTokenRequest = SecurityHelper.HashRefreshToken(request.Token);
        if (hashedPasswordResetTokenRequest != person.PasswordResetToken) return Result<bool>.Failure(new SecurityTokenException("Invalid password reset token"));
        
        if(person.PasswordResetTokenExp < DateTimeOffset.UtcNow) return Result<bool>.Failure(new ExpiredTokenException("Your token has expired. Please try again"));
        
        var hashedPassword = SecurityHelper.GetHashedPasswordAndSalt(request.Password, configuration);

        person.Password = hashedPassword.Item1;
        person.Salt = hashedPassword.Item2;
        person.PasswordResetToken = null;
        person.PasswordResetTokenExp = null;
        person.RefreshToken = "";
        person.RefreshTokenExp = DateTimeOffset.UtcNow.AddDays(-1);
        
        dbContext.Persons.Update(person);
        await dbContext.SaveChangesAsync(cancellationToken);
        
        return Result<bool>.Ok(true);
    }
}