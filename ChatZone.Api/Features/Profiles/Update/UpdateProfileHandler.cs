using System.IdentityModel.Tokens.Jwt;
using ChatZone.Context;
using ChatZone.Core.Extensions;
using ChatZone.Core.Extensions.Exceptions;
using ChatZone.Security;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ChatZone.Features.Profiles.Update;

public class UpdateProfileHandler(
    ChatZoneDbContext dbContext,
    IHttpContextAccessor httpContextAccessor,
    IToken token) : IRequestHandler<UpdateProfileRequest, Result<UpdateProfileResponse>>
{
    public async Task<Result<UpdateProfileResponse>> Handle(UpdateProfileRequest request, CancellationToken cancellationToken)
    {
        var person = await dbContext.Persons.SingleOrDefaultAsync(x => x.IdPerson == request.Id, cancellationToken);
        if(person is null) return Result<UpdateProfileResponse>.Failure(new NotFoundException("User is not found!"));
        
        bool isUsernameChanged = request.Username != person.Username;
        
        //is username changed and doesn't exist in database
        if (isUsernameChanged)
        {
            var isUsernameIsNotUsed = await dbContext.Persons.AnyAsync(x=>x.Username == request.Username, cancellationToken);
            if (isUsernameIsNotUsed) return Result<UpdateProfileResponse>.Failure(new IsExistsException("This username is exists!"));
            
            person.Username = request.Username;
        }
        person.IsFindByProfile = request.IsFindByProfile;
        
        dbContext.Persons.Update(person);
        await dbContext.SaveChangesAsync(cancellationToken);

        if (isUsernameChanged)
        {
            return Result<UpdateProfileResponse>.Ok(new UpdateProfileResponse
            {
                Username = person.Username,
                Email = person.Email,
                IsFindByProfile = person.IsFindByProfile,
                AccessToken = new JwtSecurityTokenHandler().WriteToken(token.GenerateJwtToken(person.Username, person.Role, person.IdPerson)),
                RefreshToken = person.RefreshToken
            });
        }

        return Result<UpdateProfileResponse>.Ok(new UpdateProfileResponse
        {
            Username = person.Username,
            Email = person.Email,
            IsFindByProfile = person.IsFindByProfile
        });
    }
}