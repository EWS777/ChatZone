using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using ChatZone.Context;
using ChatZone.Core.Models;
using ChatZone.Core.Models.Enums;
using ChatZone.Core.DTO.Requests;
using ChatZone.Core.Extensions;
using ChatZone.Core.Extensions.Exceptions;
using ChatZone.Core.Notifications;
using ChatZone.Repositories.Interfaces;
using ChatZone.Security;
using ChatZone.Services.Interfaces;
using Microsoft.IdentityModel.Tokens;

namespace ChatZone.Services;

public class AuthService(
    IAuthRepository authRepository,
    IConfiguration configuration,
    ChatZoneDbContext dbContext) : IAuthService {
    public async Task<Result> RegisterPersonAsync(RegisterRequest request)
    {
        try
        { 
            using var transaction = await dbContext.Database.BeginTransactionAsync();

            var getEmailResult = await authRepository.GetPersonByEmailAsync(request.Email);
            
            if (getEmailResult is { IsSuccess: true, Exception: null })
                return Result.FailResult(new ExistPersonException("The email is exist!"));
            
            var getUsernameResult = await authRepository.GetPersonByUsernameAsync(request.Username);

            if (getUsernameResult is { IsSuccess: true, Exception: null })
                return Result.FailResult(new ExistPersonException("The username is exist!"));
            
            var getHashedPasswordAndSalt = SecurityHelper.GetHashedPasswordAndSalt(request.Password);

            var person = new Person
            {
                Role = PersonRole.Unconfirmed,
                Username = request.Username,
                Email = request.Email,
                Password = getHashedPasswordAndSalt.Item1,
                Salt = getHashedPasswordAndSalt.Item2,
                RefreshToken = SecurityHelper.GenerateRefreshToken(),
                RefreshTokenExp = DateTime.Now.AddDays(1)
            };

            await authRepository.AddPersonAsync(person);
            
            var userClaim = new[]
            {
                new Claim("Username", person.Username),
                new Claim("Role", person.Role.ToString()),
                new Claim("Status", "Register"), //only before to confirm the email
                new Claim("Expires", DateTime.Now.AddMinutes(3).ToString(CultureInfo.InvariantCulture))
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:SecretKey"]));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: configuration["JWT:Issuer"],
                audience: configuration["JWT:Audience"],
                claims: userClaim,
                expires: DateTime.Now.AddMinutes(10),
                signingCredentials: creds
            );

            EmailSender.SendCodeToEmail(person.Email, new JwtSecurityTokenHandler().WriteToken(token));
            
            await transaction.CommitAsync();

            return Result.Ok();

        }
        catch (Exception e)
        {
            return Result.FailResult(e);
        }
    }
}