using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
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
            expires: DateTime.Now.AddDays(100),
            signingCredentials: creds
        );
    }
}