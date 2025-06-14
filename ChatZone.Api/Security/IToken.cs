using System.IdentityModel.Tokens.Jwt;
using ChatZone.Core.Models.Enums;

namespace ChatZone.Security;

public interface IToken
{
    JwtSecurityToken GenerateJwtToken(string username, PersonRole role, int idPerson);
}