using ChatZone.Shared.Context;
using ChatZone.Shared.DTOs;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace ChatZone.Features.Locations.GetLanguages;

public class GetLanguagesHandler(ChatZoneDbContext dbContext, IMemoryCache cache) : IRequestHandler<GetLanguagesRequest, Result<List<GetLanguagesResponse>>>
{
    public async Task<Result<List<GetLanguagesResponse>>> Handle(GetLanguagesRequest request, CancellationToken cancellationToken)
    {
        var languages = await cache.GetOrCreateAsync("languages", entry =>
        {
            entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromDays(1);
            return dbContext.Languages
                .AsNoTracking()
                .OrderBy(x => x.Name)
                .Select(x => new GetLanguagesResponse { IdLanguage = x.IdLanguage, Name = x.Name })
                .ToListAsync(cancellationToken);
        });

        return Result<List<GetLanguagesResponse>>.Ok(languages!);
    }
}
