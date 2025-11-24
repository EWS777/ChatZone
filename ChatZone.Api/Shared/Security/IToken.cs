using System.IdentityModel.Tokens.Jwt;
using ChatZone.Core.Models.Enums;

namespace ChatZone.Shared.Security;

public interface IToken
{
    JwtSecurityToken GenerateJwtToken(string username, PersonRole role, int idPerson);
    string GenerateAuthorizationToken();
}