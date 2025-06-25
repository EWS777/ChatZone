using ChatZone.Context;
using ChatZone.Core.Extensions;
using ChatZone.Core.Extensions.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ChatZone.Features.BlockedPersons.GetList;

public class GetBlockedPersonsHandler(
    ChatZoneDbContext dbContext) : IRequestHandler<GetBlockedPersonsRequest, Result<List<GetBlockedPersonsResponse>>>
{
    public async Task<Result<List<GetBlockedPersonsResponse>>> Handle(GetBlockedPersonsRequest request, CancellationToken cancellationToken)
    {
        var blockedPersons = await dbContext.BlockedPeoples
            .AsNoTracking()
            .Where(x => x.IdBlockerPerson == request.Id)
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