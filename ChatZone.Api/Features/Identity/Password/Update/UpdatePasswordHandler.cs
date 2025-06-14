using System.IdentityModel.Tokens.Jwt;
using ChatZone.Context;
using ChatZone.Core.Extensions;
using ChatZone.Core.Extensions.Exceptions;
using ChatZone.Security;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ChatZone.Features.Identity.Password.Update;

public class UpdatePasswordHandler(
    ChatZoneDbContext dbContext,
    IToken token) : IRequestHandler<UpdatePasswordRequest, Result<UpdatePasswordResponse>>
{
    public async Task<Result<UpdatePasswordResponse>> Handle(UpdatePasswordRequest request, CancellationToken cancellationToken)
    {
        var person = await dbContext.Persons.SingleOrDefaultAsync(x=>x.IdPerson == request.Id, cancellationToken);
        if (person is null) return Result<UpdatePasswordResponse>.Failure(new NotFoundException("User is not found!"));

        var currentHashedPassword = SecurityHelper.GetHashedPasswordWithSalt(request.Password, person.Salt);

        if (currentHashedPassword == person.Password)
            return Result<UpdatePasswordResponse>.Failure(new ForbiddenAccessException("You can't change, because the password is the same!"));

        person.Password = request.Password;
        dbContext.Persons.Update(person);
        await dbContext.SaveChangesAsync(cancellationToken);
        
        var generatedToken = token.GenerateJwtToken(person.Username, person.Role, person.IdPerson);
        
        return Result<UpdatePasswordResponse>.Ok(new UpdatePasswordResponse{
            AccessToken = new JwtSecurityTokenHandler().WriteToken(generatedToken),
            RefreshToken = person.RefreshToken
        });
    }
}