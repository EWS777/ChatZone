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
        var person = await dbContext.Persons.SingleOrDefaultAsync(x=>x.IdPerson == request.Id, cancellationToken);
        
        if (person is null) return Result<ConfirmResponse>.Failure(new NotFoundException("User is not found!"));

        person.Role = PersonRole.User;
        dbContext.Persons.Update(person);
        await dbContext.SaveChangesAsync(cancellationToken);
        
        var generatedToken = token.GenerateJwtToken(person.Username, person.Role, person.IdPerson);

        return Result<ConfirmResponse>.Ok(new ConfirmResponse{
            AccessToken = new JwtSecurityTokenHandler().WriteToken(generatedToken),
            RefreshToken = person.RefreshToken
        });
    }
}