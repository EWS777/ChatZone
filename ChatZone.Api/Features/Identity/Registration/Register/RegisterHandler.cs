using ChatZone.Core.Extensions.Exceptions;
using ChatZone.Core.Models;
using ChatZone.Core.Models.Enums;
using ChatZone.Core.Notifications;
using ChatZone.Shared.Context;
using ChatZone.Shared.DTOs;
using ChatZone.Shared.Security;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ChatZone.Features.Identity.Registration.Register;

public class RegisterHandler(ChatZoneDbContext dbContext) : IRequestHandler<RegisterRequest, Result<bool>>
{
    public async Task<Result<bool>> Handle(RegisterRequest request, CancellationToken cancellationToken)
    {
        var isEmailOrUsernameExists =
            await dbContext.Persons.AnyAsync(x => x.Email == request.Email || x.Username == request.Username, cancellationToken);
        if(isEmailOrUsernameExists) return Result<bool>.Failure(new ExistPersonException("The email or username already exists."));
        
        var getHashedPasswordAndSalt = SecurityHelper.GetHashedPasswordAndSalt(request.Password);
        
        var person = new Person
        {
            Role = PersonRole.Unconfirmed,
            Username = request.Username,
            Email = request.Email,
            Password = getHashedPasswordAndSalt.Item1,
            Salt = getHashedPasswordAndSalt.Item2,
            RefreshToken = "",
            RefreshTokenExp = DateTimeOffset.UtcNow,
            EmailConfirmToken = SecurityHelper.GenerateEmailAuthorizationToken(),
            EmailConfirmTokenExp = DateTimeOffset.UtcNow.AddMinutes(15)
        };
        
        await dbContext.Persons.AddAsync(person, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);

        await EmailSender.SendCodeToEmail(person.Email, person.EmailConfirmToken, cancellationToken);
        
        return Result<bool>.Ok(true);
    }
}