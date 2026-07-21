using ChatZone.Shared.Context;
using ChatZone.Shared.DTOs;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ChatZone.Features.BlockedPersons.GetList;

public class GetBlockedPersonsHandler(
    ChatZoneDbContext dbContext) : IRequestHandler<GetBlockedPersonsRequest, Result<List<GetBlockedPersonsResponse>>>
{
    public async Task<Result<List<GetBlockedPersonsResponse>>> Handle(GetBlockedPersonsRequest request, CancellationToken cancellationToken)
    {
        var query = dbContext.BlockedPeoples
            .AsNoTracking()
            .Where(x => x.IdBlockerPerson == request.IdPerson);

        if (request.Cursor.HasValue) query = query.Where(x => x.CreatedAt < request.Cursor.Value);
        
        var blockedPersons = await query
            .OrderByDescending(x => x.CreatedAt)
            .Take(request.TakePerson)
            .Select(x => new GetBlockedPersonsResponse
            {
                IdBlockedPerson = x.IdBlockedPerson,
                BlockedUsername = x.Blocked.Username,
                CreatedAt = x.CreatedAt
            })
            .ToListAsync(cancellationToken);
        return Result<List<GetBlockedPersonsResponse>>.Ok(blockedPersons);
    }
}