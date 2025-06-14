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
        var person = await dbContext.Persons.SingleOrDefaultAsync(x=>x.IdPerson==request.Id, cancellationToken);
        if (person is null) return Result<GetProfileResponse>.Failure(new NotFoundException("User is not found!"));

        return Result<GetProfileResponse>.Ok(new GetProfileResponse
        {
            Username = person.Username,
            Email = person.Email,
            IsFindByProfile = person.IsFindByProfile
        });
    }
}