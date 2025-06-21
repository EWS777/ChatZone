using System.IdentityModel.Tokens.Jwt;
using ChatZone.Context;
using ChatZone.Core.Extensions;
using ChatZone.Core.Extensions.Exceptions;
using ChatZone.Core.Models.Enums;
using ChatZone.Security;
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

        if (person is null) return Result<LoginResponse>.Failure(new NotFoundException("User is not found!"));
        
        if(person.Role != PersonRole.User) return Result<LoginResponse>.Failure(new ForbiddenAccessException("The email is not confirmed"));

        var currentHashedCode = SecurityHelper.GetHashedPasswordWithSalt(request.Password, person.Salt);

        if (currentHashedCode != person.Password)
            return Result<LoginResponse>.Failure(new ForbiddenAccessException("The password is not match"));

        var generatedToken = token.GenerateJwtToken(person.Username, person.Role, person.IdPerson);
        
        person.RefreshToken = SecurityHelper.GenerateRefreshToken();
        person.RefreshTokenExp = DateTimeOffset.UtcNow.AddDays(7);
        dbContext.Persons.Update(person);
        await dbContext.SaveChangesAsync(cancellationToken);
        
        return Result<LoginResponse>.Ok(new LoginResponse{
            AccessToken = new JwtSecurityTokenHandler().WriteToken(generatedToken),
            RefreshToken = person.RefreshToken
        });
    }
}