using ChatZone.Context;
using ChatZone.Core.Extensions;
using ChatZone.Core.Extensions.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ChatZone.Features.Profiles.Get;

public class GetProfileHandler(ChatZoneDbContext dbContext) : IRequestHandler<GetProfileRequest, Result<GetProfileResponse>>
{
    public async Task<Result<GetProfileResponse>> Handle(GetProfileRequest request, CancellationToken cancellationToken)
    {
        var person = await dbContext.Persons
            .AsNoTracking()
            .Where(x => x.IdPerson == request.Id)
            .Select(x => new GetProfileResponse
            {
                Username = x.Username,
                Email = x.Email,
                IsFindByProfile = x.IsFindByProfile
            })
            .SingleOrDefaultAsync(cancellationToken);
        
        return person is null ? Result<GetProfileResponse>.Failure(new NotFoundException("User is not found!")) : Result<GetProfileResponse>.Ok(person);
    }
}