using ChatZone.Core.Extensions.Exceptions;
using ChatZone.Core.Notifications;
using ChatZone.Shared.Context;
using ChatZone.Shared.DTOs;
using ChatZone.Shared.Security;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ChatZone.Features.Identity.Registration.Reconfirm;

public class ReconfirmHandler(
    ChatZoneDbContext dbContext,
    IConfiguration configuration,
    EmailSender emailSender) : IRequestHandler<ReconfirmRequest, Result<bool>>
{
    public async Task<Result<bool>> Handle(ReconfirmRequest request, CancellationToken cancellationToken)
    {
        var person = await dbContext.Persons.SingleOrDefaultAsync(x => x.Email == request.Email, cancellationToken);
        if(person is null) return Result<bool>.Failure(new NotFoundException("User is not found!"));

        var emailConfirmToken = SecurityHelper.GenerateRefreshToken();
        person.EmailConfirmToken = SecurityHelper.HashRefreshToken(emailConfirmToken);
        person.EmailConfirmTokenExp = DateTimeOffset.UtcNow.AddMinutes(double.Parse(configuration["JWT:EmailConfirmTokenExpMinutes"]!));

        await using var transaction = await dbContext.Database.BeginTransactionAsync(cancellationToken);
        try
        {
            dbContext.Persons.Update(person);
            await dbContext.SaveChangesAsync(cancellationToken);
            
            await emailSender.SendCodeToEmail(person.Email, emailConfirmToken, cancellationToken);
            
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