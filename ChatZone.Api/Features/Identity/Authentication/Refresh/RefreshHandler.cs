using System.IdentityModel.Tokens.Jwt;
using ChatZone.Core.Extensions.Exceptions;
using ChatZone.Shared.Context;
using ChatZone.Shared.DTOs;
using ChatZone.Shared.Security;
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
        var person = await dbContext.Persons.SingleOrDefaultAsync(x=>x.IdPerson == request.IdPerson, cancellationToken);
        if (person is null) return Result<RefreshResponse>.Failure(new NotFoundException("User is not found!"));

        if (person.RefreshTokenExp < DateTimeOffset.UtcNow)
            return Result<RefreshResponse>.Failure(new SecurityTokenException("Refresh token expired"));
        
        var generatedAccessToken = token.GenerateJwtToken(person.Username, person.Role, person.IdPerson);

        person.RefreshTokenExp = DateTimeOffset.UtcNow.AddDays(7);
        dbContext.Persons.Update(person);
        await dbContext.SaveChangesAsync(cancellationToken);
        
        return Result<RefreshResponse>.Ok(new RefreshResponse{
            AccessToken = new JwtSecurityTokenHandler().WriteToken(generatedAccessToken),
            RefreshToken = person.RefreshToken
        });
    }
}