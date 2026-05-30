using ChatZone.Core.Extensions.Exceptions;
using ChatZone.Core.Notifications;
using ChatZone.Shared.Context;
using ChatZone.Shared.DTOs;
using ChatZone.Shared.Security;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ChatZone.Features.Identity.Password.Reset;

public class ResetPasswordHandler(ChatZoneDbContext dbContext,
    IConfiguration configuration) : IRequestHandler<ResetPasswordRequest, Result<bool>>
{
    public async Task<Result<bool>> Handle(ResetPasswordRequest request, CancellationToken cancellationToken)
    {
        var person = await dbContext.Persons.SingleOrDefaultAsync(x=>x.Email==request.Email, cancellationToken);
        if (person is null) return Result<bool>.Failure(new NotFoundException("User is not found!"));

        var passwordResetToken = SecurityHelper.GenerateRefreshToken();
        person.PasswordResetToken = SecurityHelper.HashRefreshToken(passwordResetToken);
        person.PasswordResetTokenExp = DateTimeOffset.UtcNow.AddMinutes(double.Parse(configuration["JWT:ResetPasswordTokenExpMinutes"]!));
        
        await using var transaction = await dbContext.Database.BeginTransactionAsync(cancellationToken);
        try
        {
            dbContext.Persons.Update(person);
            await dbContext.SaveChangesAsync(cancellationToken);
            
            await EmailSender.ResetPassword(person.Email, passwordResetToken, cancellationToken);
            
            await transaction.CommitAsync(cancellationToken);
            
            return Result<bool>.Ok(true);
        }
        catch (Exception)
        {
            await transaction.RollbackAsync(cancellationToken);
            return Result<bool>.Failure(new BackendException());
        }
    }
}