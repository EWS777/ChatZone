using ChatZone.Context;
using ChatZone.Core.Extensions;
using ChatZone.Core.Extensions.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ChatZone.Features.Filters.Get;

public class GetFilterHandler(ChatZoneDbContext dbContext) : IRequestHandler<GetFilterRequest, Result<GetFilterResponse>>
{
    public async Task<Result<GetFilterResponse>> Handle(GetFilterRequest request, CancellationToken cancellationToken)
    {
        var person = await dbContext.Persons
            .AsNoTracking()
            .Where(x => x.IdPerson == request.Id)
            .Select(x => new GetFilterResponse
            {
                Theme = x.Theme,
                Country = x.Country,
                City = x.City,
                Age = x.Age,
                YourGender = x.YourGender,
                PartnerGender = x.PartnerGender,
                Language = x.Language,
            })
            .SingleOrDefaultAsync(cancellationToken);
        
        return person is null ? Result<GetFilterResponse>.Failure(new NotFoundException("User is not found!")) : Result<GetFilterResponse>.Ok(person);
    }
}