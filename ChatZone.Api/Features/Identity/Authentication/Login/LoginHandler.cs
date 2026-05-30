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
    IToken token,
    IConfiguration configuration) : IRequestHandler<LoginRequest, Result<LoginResponse>>
{
    public async Task<Result<LoginResponse>> Handle(LoginRequest request, CancellationToken cancellationToken)
    {
        var validation = await validator.ValidateAsync(request, cancellationToken);
        if (!validation.IsValid) return Result<LoginResponse>.Failure(validation.Errors.ToList());
        
        var person = request.UsernameOrEmail.Contains('@')
            ? await dbContext.Persons.SingleOrDefaultAsync(x=>x.Email == request.UsernameOrEmail, cancellationToken)
            : await dbContext.Persons.SingleOrDefaultAsync(x=>x.Username == request.UsernameOrEmail, cancellationToken);

        if (person is null || person.Role != PersonRole.User) return Result<LoginResponse>.Failure(new ForbiddenAccessException("Email or password is not correct!"));

        var currentHashedCode = SecurityHelper.GetHashedPasswordWithSalt(request.Password, person.Salt, configuration);

        if (currentHashedCode != person.Password)
            return Result<LoginResponse>.Failure(new ForbiddenAccessException("Email or password is not correct!"));

        var generatedAccessToken = token.GenerateJwtToken(person.Username, person.Role, person.IdPerson);
        var generatedRefreshToken = SecurityHelper.GenerateRefreshToken();
        
        person.RefreshTokenExp = DateTimeOffset.UtcNow.AddDays(double.Parse(configuration["JWT:RefreshTokenExpDays"]!));
        person.RefreshToken = SecurityHelper.HashRefreshToken(generatedRefreshToken);
        
        dbContext.Persons.Update(person);
        await dbContext.SaveChangesAsync(cancellationToken);
        
        return Result<LoginResponse>.Ok(new LoginResponse{
            AccessToken = new JwtSecurityTokenHandler().WriteToken(generatedAccessToken),
            RefreshToken = generatedRefreshToken
        });
    }
}