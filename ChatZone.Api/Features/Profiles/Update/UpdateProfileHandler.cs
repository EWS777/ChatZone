using System.IdentityModel.Tokens.Jwt;
using ChatZone.Core.Extensions.Exceptions;
using ChatZone.Shared.Context;
using ChatZone.Shared.DTOs;
using ChatZone.Shared.Security;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ChatZone.Features.Profiles.Update;

public class UpdateProfileHandler(
    ChatZoneDbContext dbContext,
    IToken token,
    IConfiguration configuration) : IRequestHandler<UpdateProfileRequest, Result<UpdateProfileResponse>>
{
    public async Task<Result<UpdateProfileResponse>> Handle(UpdateProfileRequest request, CancellationToken cancellationToken)
    {
        var person = await dbContext.Persons.SingleOrDefaultAsync(x => x.IdPerson == request.IdPerson, cancellationToken);
        if(person is null) return Result<UpdateProfileResponse>.Failure(new NotFoundException("User is not found!"));
        
        bool isUsernameChanged = request.Username != person.Username;
        string? newRefreshToken = null;
        
        //is username changed and doesn't exist in database
        if (isUsernameChanged)
        {
            var isUsernameTaken = await dbContext.Persons.AnyAsync(x=>x.Username == request.Username, cancellationToken);
            if (isUsernameTaken) return Result<UpdateProfileResponse>.Failure(new IsExistsException("This username is exists!"));
            
            person.Username = request.Username;
            
            newRefreshToken = SecurityHelper.GenerateRefreshToken();
            person.RefreshToken = SecurityHelper.HashRefreshToken(newRefreshToken);
            person.RefreshTokenExp = DateTimeOffset.UtcNow.AddDays(double.Parse(configuration["JWT:RefreshTokenExpDays"]!));
        }
        
        dbContext.Persons.Update(person);
        await dbContext.SaveChangesAsync(cancellationToken);

        if (isUsernameChanged)
        {
            return Result<UpdateProfileResponse>.Ok(new UpdateProfileResponse
            {
                Username = person.Username,
                Email = person.Email,
                AccessToken = new JwtSecurityTokenHandler().WriteToken(token.GenerateJwtToken(person.Username, person.Role, person.IdPerson)),
                RefreshToken = newRefreshToken
            });
        }

        return Result<UpdateProfileResponse>.Ok(new UpdateProfileResponse
        {
            Username = person.Username,
            Email = person.Email
        });
    }
}