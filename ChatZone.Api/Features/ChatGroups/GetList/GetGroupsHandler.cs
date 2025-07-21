using ChatZone.Context;
using ChatZone.Core.Extensions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ChatZone.Features.ChatGroups.GetList;

public class GetGroupsHandler(
    ChatZoneDbContext dbContext) : IRequestHandler<GetGroupsRequest, Result<List<GetGroupsResponse>>>
{
    public async Task<Result<List<GetGroupsResponse>>> Handle(GetGroupsRequest request, CancellationToken cancellationToken)
    {
        return Result<List<GetGroupsResponse>>.Ok(
            await dbContext.GroupChats
                .AsNoTracking()
                .Select(x => new GetGroupsResponse
                {
                    IdGroup = x.IdGroupChat,
                    Title = x.Title,
                    Country = x.Country,
                    City = x.City,
                    Age = x.Age,
                    Lang = x.Lang
                }).ToListAsync(cancellationToken));
    }
}