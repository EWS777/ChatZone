using ChatZone.Context;
using ChatZone.Core.Extensions;
using ChatZone.Core.Extensions.Exceptions;
using ChatZone.Core.Models;
using ChatZone.Core.Models.Enums;
using ChatZone.Core.Notifications;
using ChatZone.Security;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ChatZone.Features.Identity.Registration.Register;

public class RegisterHandler(
    ChatZoneDbContext dbContext,
    IToken token) : IRequestHandler<RegisterRequest, Result<IActionResult>>
{
    public async Task<Result<IActionResult>> Handle(RegisterRequest request, CancellationToken cancellationToken)
    {
        Person person;
        try
        {
            await using var transaction = await dbContext.Database.BeginTransactionAsync(cancellationToken);

            var getEmailResult = await dbContext.Persons.AnyAsync(x=>x.Email == request.Email, cancellationToken);

            if (getEmailResult) return Result<IActionResult>.Failure(new ExistPersonException("The email is exist!"));
            
            var getUsernameResult = await dbContext.Persons.AnyAsync(x=>x.Username == request.Username, cancellationToken);
            
            if (getUsernameResult) return Result<IActionResult>.Failure(new ExistPersonException("The username is exist!"));
            
            var getHashedPasswordAndSalt = SecurityHelper.GetHashedPasswordAndSalt(request.Password);
            
            person = new Person
            {
                Role = PersonRole.Unconfirmed,
                Username = request.Username,
                Email = request.Email,
                Password = getHashedPasswordAndSalt.Item1,
                Salt = getHashedPasswordAndSalt.Item2,
                RefreshToken = SecurityHelper.GenerateRefreshToken(),
                RefreshTokenExp = DateTimeOffset.UtcNow.AddDays(7),
                EmailConfirmToken = token.GenerateAuthorizationToken(),
                EmailConfirmTokenExp = DateTimeOffset.UtcNow.AddMinutes(15)
            };
            
            await dbContext.Persons.AddAsync(person, cancellationToken);
            await dbContext.SaveChangesAsync(cancellationToken);
            
            await transaction.CommitAsync(cancellationToken);
        }
        catch (Exception e)
        {
            return Result<IActionResult>.Failure(e);
        }
        
        await EmailSender.SendCodeToEmail(person.Email, person.EmailConfirmToken, cancellationToken);
        
        return Result<IActionResult>.Ok(new OkResult());
    }
}