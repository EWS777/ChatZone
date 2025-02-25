using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using ChatZone.Core.Models;
using ChatZone.Core.Models.Enums;
using ChatZone.Core.DTO.Requests;
using ChatZone.Core.DTO.Responses;
using ChatZone.Repositories.Interfaces;
using ChatZone.Security;
using ChatZone.Services.Interfaces;
using Microsoft.IdentityModel.Tokens;

namespace ChatZone.Services;

public class AuthService(
    IAuthRepository authRepository,
    IConfiguration configuration) : IAuthService {
    public async Task<RegisterResponse> RegisterPersonAsync(RegisterRequest request)
    {
        var getEmailResult = await authRepository.GetPersonByEmailAsync(request.Email);
        if (getEmailResult is not null) ; //return that is email exist 

        var getUsernameResult = await authRepository.GetPersonByUsernameAsync(request.Username);
        if (getUsernameResult is not null) ; //return that is username exist

        var getHashedPasswordAndSalt = SecurityHelper.GetHashedPasswordAndSalt(request.Password);

        var person = new Person
        {
            Role = PersonRole.User,
            Username = request.Username,
            Email = request.Username,
            Password = getHashedPasswordAndSalt.Item1,
            Salt = getHashedPasswordAndSalt.Item2,
            RefreshToken = SecurityHelper.GenerateRefreshToken(),
            RefreshTokenExp = DateTime.Now.AddDays(7)
        };

        await authRepository.RegisterPersonAsync(person);

        var userClaim = new[]
        {
            new Claim("Username", person.Username),
            new Claim("Role", person.Role.ToString())
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

        return new RegisterResponse
        {
            AccessToken = new JwtSecurityTokenHandler().WriteToken(token),
            RefreshToken = person.RefreshToken
        };
    }
}