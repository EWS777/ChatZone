using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using ChatZone.Core.Models.Enums;
using Microsoft.IdentityModel.Tokens;

namespace ChatZone.Security;

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

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:SecretKey"]));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        return new JwtSecurityToken(
            issuer: configuration["JWT:Issuer"],
            audience: configuration["JWT:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddDays(100),
            signingCredentials: creds
        );
    }

    public string GenerateAuthorizationToken()
    {
        const string chars = "abcdefghijklmnopqrstuvwxyz0123456789";
        const int length = 70;
        using var rng = RandomNumberGenerator.Create();
        var bytes = new byte[length];
        rng.GetBytes(bytes);
        var result = new char[length];
        for (int i = 0; i < length; i++)
        {
            result[i] = chars[bytes[i] % chars.Length];
        }
        return new string(result);
    }
}