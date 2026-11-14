using ChatZone.Shared.Context;
using ChatZone.Shared.DTOs;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace ChatZone.Features.Locations.GetCountries;

public class GetCountriesHandler(ChatZoneDbContext dbContext, IMemoryCache cache) : IRequestHandler<GetCountriesRequest, Result<List<GetCountriesResponse>>>
{
    public async Task<Result<List<GetCountriesResponse>>> Handle(GetCountriesRequest request, CancellationToken cancellationToken)
    {
        var countries = await cache.GetOrCreateAsync("countries", entry =>
        {
            entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromDays(1);
            return dbContext.Countries
                .AsNoTracking()
                .OrderBy(x => x.Name)
                .Select(x => new GetCountriesResponse { IdCountry = x.IdCountry, Name = x.Name })
                .ToListAsync(cancellationToken);
        });

        return Result<List<GetCountriesResponse>>.Ok(countries!);
    }
}
