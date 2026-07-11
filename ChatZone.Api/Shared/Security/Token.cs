using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using ChatZone.Core.Models.Enums;
using Microsoft.IdentityModel.Tokens;

namespace ChatZone.Shared.Security;

public class Token(IConfiguration configuration) : IToken
{
    public JwtSecurityToken GenerateJwtToken(string username, PersonRole role, int idPerson)
    {
        var claims = new[]
        {
            new Claim(ClaimTypes.Name, username),
            new Claim(ClaimTypes.Role, role.ToString()),
            new Claim(ClaimTypes.NameIdentifier, idPerson.ToString())
        };

        var secretKey = configuration["JWT:SecretKey"] ?? throw new InvalidOperationException("JWT Secret Key is not configured.");
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        return new JwtSecurityToken(
            issuer: configuration["JWT:Issuer"],
            audience: configuration["JWT:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(double.Parse(configuration["JWT:AccessTokenExpMinutes"]!)),
            signingCredentials: creds
        );
    }
}