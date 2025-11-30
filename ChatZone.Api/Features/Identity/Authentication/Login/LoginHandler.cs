using System.IdentityModel.Tokens.Jwt;
using ChatZone.Core.Extensions.Exceptions;
using ChatZone.Core.Models.Enums;
using ChatZone.Shared.Context;
using ChatZone.Shared.DTOs;
using ChatZone.Shared.Security;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ChatZone.Features.Identity.Authentication.Login;

public class LoginHandler(
    ChatZoneDbContext dbContext,
    IValidator<LoginRequest> validator, 
    IToken token) : IRequestHandler<LoginRequest, Result<LoginResponse>>
{
    public async Task<Result<LoginResponse>> Handle(LoginRequest request, CancellationToken cancellationToken)
    {
        var validation = await validator.ValidateAsync(request, cancellationToken);
        if (!validation.IsValid) return Result<LoginResponse>.Failure(validation.Errors.ToList());
        
        var person = request.UsernameOrEmail.Contains("@")
            ? await dbContext.Persons.SingleOrDefaultAsync(x=>x.Email == request.UsernameOrEmail, cancellationToken)
            : await dbContext.Persons.SingleOrDefaultAsync(x=>x.Username == request.UsernameOrEmail, cancellationToken);

        if (person is null) return Result<LoginResponse>.Failure(new NotFoundException("Email or password is not correct!"));
        
        if(person.Role != PersonRole.User) return Result<LoginResponse>.Failure(new ForbiddenAccessException("Email or password is not correct!"));

        var currentHashedCode = SecurityHelper.GetHashedPasswordWithSalt(request.Password, person.Salt);

        if (currentHashedCode != person.Password)
            return Result<LoginResponse>.Failure(new ForbiddenAccessException("Email or password is not correct!"));

        var generatedAccessToken = token.GenerateJwtToken(person.Username, person.Role, person.IdPerson);
        var generatedRefreshToken = token.GenerateJwtToken(person.Username, person.Role, person.IdPerson);
        
        person.RefreshTokenExp = DateTimeOffset.UtcNow.AddDays(7);
        person.RefreshToken = generatedRefreshToken.ToString();
        
        dbContext.Persons.Update(person);
        await dbContext.SaveChangesAsync(cancellationToken);
        
        return Result<LoginResponse>.Ok(new LoginResponse{
            AccessToken = new JwtSecurityTokenHandler().WriteToken(generatedAccessToken),
            RefreshToken = new JwtSecurityTokenHandler().WriteToken(generatedRefreshToken)
        });
    }
}