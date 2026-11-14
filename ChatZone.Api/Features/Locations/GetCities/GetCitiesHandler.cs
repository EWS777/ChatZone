using ChatZone.Core.Extensions.Exceptions;
using ChatZone.Shared.Context;
using ChatZone.Shared.DTOs;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ChatZone.Features.Locations.GetCities;

public class GetCitiesHandler(ChatZoneDbContext dbContext) : IRequestHandler<GetCitiesRequest, Result<List<GetCitiesResponse>>>
{
    public async Task<Result<List<GetCitiesResponse>>> Handle(GetCitiesRequest request, CancellationToken cancellationToken)
    {
        var isCountryExists = await dbContext.Countries.AnyAsync(x => x.IdCountry == request.IdCountry, cancellationToken);
        if (!isCountryExists) return Result<List<GetCitiesResponse>>.Failure(new NotFoundException("Country is not found!"));

        var query = dbContext.Cities.AsNoTracking().Where(x => x.IdCountry == request.IdCountry);

        var search = request.Search?.Trim();
        if (!string.IsNullOrEmpty(search)) query = query.Where(x => x.Name.StartsWith(search));
        
        var cities = await query
            .OrderByDescending(x => x.Population)
            .ThenBy(x => x.Name)
            .Take(request.Take)
            .Select(x => new GetCitiesResponse { IdCity = x.IdCity, Name = x.Name })
            .ToListAsync(cancellationToken);

        return Result<List<GetCitiesResponse>>.Ok(cities);
    }
}
