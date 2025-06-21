using System.IdentityModel.Tokens.Jwt;
using ChatZone.Context;
using ChatZone.Core.Extensions;
using ChatZone.Core.Extensions.Exceptions;
using ChatZone.Core.Models.Enums;
using ChatZone.Security;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ChatZone.Features.Identity.Registration.Confirm;

public class ConfirmHandler(
    ChatZoneDbContext dbContext,
    IToken token) : IRequestHandler<ConfirmRequest, Result<ConfirmResponse>>
{
    public async Task<Result<ConfirmResponse>> Handle(ConfirmRequest request, CancellationToken cancellationToken)
    {
        var person = await dbContext.Persons.SingleOrDefaultAsync(x => x.EmailConfirmToken == request.Token, cancellationToken);
        
        if (person is null) return Result<ConfirmResponse>.Failure(new NotFoundException("Token is not exists!"));
        if (person.EmailConfirmTokenExp < DateTimeOffset.UtcNow) return Result<ConfirmResponse>.Failure(new ExpiredTokenException("Token was expired!"));
        
        person.Role = PersonRole.User;
        person.EmailConfirmToken = null;
        person.EmailConfirmTokenExp = null;
        dbContext.Persons.Update(person);
        
        await dbContext.SaveChangesAsync(cancellationToken);
        
        var generatedToken = token.GenerateJwtToken(person.Username, person.Role, person.IdPerson);

        return Result<ConfirmResponse>.Ok(new ConfirmResponse{
            AccessToken = new JwtSecurityTokenHandler().WriteToken(generatedToken),
            RefreshToken = person.RefreshToken
        });
    }
}