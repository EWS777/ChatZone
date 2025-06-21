using System.IdentityModel.Tokens.Jwt;
using ChatZone.Context;
using ChatZone.Core.Extensions;
using ChatZone.Core.Extensions.Exceptions;
using ChatZone.Security;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace ChatZone.Features.Identity.Authentication.Refresh;

public class RefreshHandler(
    ChatZoneDbContext dbContext,
    IToken token) : IRequestHandler<RefreshRequest, Result<RefreshResponse>>
{
    public async Task<Result<RefreshResponse>> Handle(RefreshRequest request, CancellationToken cancellationToken)
    {
        var person = await dbContext.Persons.SingleOrDefaultAsync(x=>x.IdPerson == request.Id, cancellationToken);
        if (person is null) return Result<RefreshResponse>.Failure(new NotFoundException("User is not found!"));

        if (person.RefreshTokenExp < DateTimeOffset.UtcNow)
            return Result<RefreshResponse>.Failure(new SecurityTokenException("Refresh token expired"));
        
        var generatedToken = token.GenerateJwtToken(person.Username, person.Role, person.IdPerson);

        person.RefreshToken = SecurityHelper.GenerateRefreshToken();
        person.RefreshTokenExp = DateTimeOffset.UtcNow.AddDays(7);
        dbContext.Persons.Update(person);
        await dbContext.SaveChangesAsync(cancellationToken);
        
        return Result<RefreshResponse>.Ok(new RefreshResponse{
            AccessToken = new JwtSecurityTokenHandler().WriteToken(generatedToken),
            RefreshToken = person.RefreshToken
        });
    }
}