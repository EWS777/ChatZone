using ChatZone.Core.Extensions.Exceptions;
using ChatZone.Shared.Context;
using ChatZone.Shared.DTOs;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ChatZone.Features.Filters.Get;

public class GetFilterHandler(ChatZoneDbContext dbContext) : IRequestHandler<GetFilterRequest, Result<GetFilterResponse>>
{
    public async Task<Result<GetFilterResponse>> Handle(GetFilterRequest request, CancellationToken cancellationToken)
    {
        var person = await dbContext.Persons
            .AsNoTracking()
            .Where(x => x.IdPerson == request.IdPerson)
            .Select(x => new GetFilterResponse
            {
                Theme = x.Theme,
                Country = x.Country == null ? null : new NamedItemDto { Id = x.Country.IdCountry, Name = x.Country.Name },
                City = x.City == null ? null : new NamedItemDto { Id = x.City.IdCity, Name = x.City.Name },
                Age = x.Age,
                YourGender = x.YourGender,
                PartnerGender = x.PartnerGender,
                Language = x.Language == null ? null : new NamedItemDto { Id = x.Language.IdLanguage, Name = x.Language.Name }
            })
            .SingleOrDefaultAsync(cancellationToken);
        
        return person is null
            ? Result<GetFilterResponse>.Failure(new NotFoundException("User is not found!"))
            : Result<GetFilterResponse>.Ok(person);
    }
}