using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using ChatZone.Context;
using ChatZone.Core.Models;
using ChatZone.Core.Models.Enums;
using ChatZone.Core.Extensions;
using ChatZone.Core.Extensions.Exceptions;
using ChatZone.Core.Notifications;
using ChatZone.DTO.Requests;
using ChatZone.DTO.Responses;
using ChatZone.Repositories.Interfaces;
using ChatZone.Security;
using ChatZone.Services.Interfaces;
using Microsoft.IdentityModel.Tokens;

namespace ChatZone.Services;

public class AuthService(
    IAuthRepository authRepository,
    IConfiguration configuration,
    ChatZoneDbContext dbContext) : IAuthService {
    public async Task<Result<bool>> RegisterPersonAsync(RegisterRequest request)
    {
        try
        { 
            using var transaction = await dbContext.Database.BeginTransactionAsync();

            var getEmailResult = await authRepository.GetPersonByEmailAsync(request.Email);

            if (getEmailResult.IsSuccess) return Result<bool>.Failure(new ExistPersonException("The email is exist!"));
            
            var getUsernameResult = await authRepository.GetPersonByUsernameAsync(request.Username);
            
            if (getUsernameResult.IsSuccess) return Result<bool>.Failure(new ExistPersonException("The username is exist!"));
            
            var getHashedPasswordAndSalt = SecurityHelper.GetHashedPasswordAndSalt(request.Password);

            var person = new Person
            {
                Role = PersonRole.Unconfirmed,
                Username = request.Username,
                Email = request.Email,
                Password = getHashedPasswordAndSalt.Item1,
                Salt = getHashedPasswordAndSalt.Item2,
                RefreshToken = SecurityHelper.GenerateRefreshToken(),
                RefreshTokenExp = DateTime.Now.AddDays(7)
            };

            await authRepository.AddPersonAsync(person);
            
            var token = GenerateJwtToken(person.Username, person.Role);

            EmailSender.SendCodeToEmail(person.Email, new JwtSecurityTokenHandler().WriteToken(token));
            
            await transaction.CommitAsync();

            return Result<bool>.Ok(true);

        }
        catch (Exception e)
        {
            return Result<bool>.Failure(e);
        }
    }

    public async Task<Result<RegisterResponse>> ConfirmEmailAsync(string username)
    {
        var result = await authRepository.GetPersonByUsernameAsync(username);
        
        if (!result.IsSuccess) return Result<RegisterResponse>.Failure(result.Exception);
        
        var updatePerson = await authRepository.UpdatePersonAsync(username, PersonRole.User);
        
        var token = GenerateJwtToken(updatePerson.Value.Username, updatePerson.Value.Role);

        return Result<RegisterResponse>.Ok(new RegisterResponse{
            AccessToken = new JwtSecurityTokenHandler().WriteToken(token),
            RefreshToken = updatePerson.Value.RefreshToken
        });
    }

    public async Task<Result<RegisterResponse>> LoginAsync(string usernameOrEmail, string password)
    {
        var user = usernameOrEmail.Contains("@")
            ? await authRepository.GetPersonByEmailAsync(usernameOrEmail)
            : await authRepository.GetPersonByUsernameAsync(usernameOrEmail);

        if (!user.IsSuccess) return Result<RegisterResponse>.Failure(user.Exception);
        
        if(user.Value.Role != PersonRole.User) return Result<RegisterResponse>.Failure(new ForbiddenAccessException("The email is not confirmed"));

        var currentHashedCode = SecurityHelper.GetHashedPasswordWithSalt(password, user.Value.Salt);

        if (currentHashedCode != user.Value.Password)
            return Result<RegisterResponse>.Failure(new ForbiddenAccessException("The password is not match"));

        var token = GenerateJwtToken(user.Value.Username, user.Value.Role);

        var person = await authRepository.UpdatePersonTokenAsync(user.Value.Username,
            SecurityHelper.GenerateRefreshToken(), DateTime.Now.AddDays(7));
        
        return Result<RegisterResponse>.Ok(new RegisterResponse{
            AccessToken = new JwtSecurityTokenHandler().WriteToken(token),
            RefreshToken = person.Value.RefreshToken
        });
    }

    public async Task<Result<RegisterResponse>> RefreshTokenAsync(string username)
    {
        var user = await authRepository.GetPersonByUsernameAsync(username);
        if (!user.IsSuccess) return Result<RegisterResponse>.Failure(user.Exception);

        if (user.Value.RefreshTokenExp < DateTime.Now)
            return Result<RegisterResponse>.Failure(new SecurityTokenException("Refresh token expired"));
        
        var token = GenerateJwtToken(user.Value.Username, user.Value.Role);
        
        var person = await authRepository.UpdatePersonTokenAsync(user.Value.Username,
            SecurityHelper.GenerateRefreshToken(), DateTime.Now.AddDays(7));
        
        return Result<RegisterResponse>.Ok(new RegisterResponse{
            AccessToken = new JwtSecurityTokenHandler().WriteToken(token),
            RefreshToken = person.Value.RefreshToken
        });
    }

    public async Task<Result<bool>> ResetPasswordAsync(string email)
    {
        var user = await authRepository.GetPersonByEmailAsync(email);
        if (!user.IsSuccess) return Result<bool>.Failure(user.Exception);

        var token = GenerateJwtToken(user.Value.Username, user.Value.Role);
        EmailSender.SendCodeToEmail(user.Value.Email, new JwtSecurityTokenHandler().WriteToken(token));

        return Result<bool>.Ok(true);
    }

    public async Task<Result<RegisterResponse>> UpdatePasswordAsync(string username, string password)
    {
        var user = await authRepository.GetPersonByUsernameAsync(username);
        if (!user.IsSuccess) return Result<RegisterResponse>.Failure(user.Exception);

        var currentHashedPassword = SecurityHelper.GetHashedPasswordWithSalt(password, user.Value.Salt);

        if (currentHashedPassword == user.Value.Password)
            return Result<RegisterResponse>.Failure(new ForbiddenAccessException("You can't change, because the password is the same!"));

        var updatePerson = await authRepository.UpdatePasswordAsync(username, currentHashedPassword);
        
        var token = GenerateJwtToken(user.Value.Username, user.Value.Role);
        
        return Result<RegisterResponse>.Ok(new RegisterResponse{
            AccessToken = new JwtSecurityTokenHandler().WriteToken(token),
            RefreshToken = updatePerson.Value.RefreshToken
        });
    }

    private JwtSecurityToken GenerateJwtToken(string username, PersonRole role)
    {
        var claims = new[]
        {
            new Claim(ClaimTypes.Name, username),
            new Claim(ClaimTypes.Role, role.ToString())
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:SecretKey"]));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        return new JwtSecurityToken(
            issuer: configuration["JWT:Issuer"],
            audience: configuration["JWT:Audience"],
            claims: claims,
            expires: DateTime.Now.AddDays(10),
            signingCredentials: creds
        );
    }
}