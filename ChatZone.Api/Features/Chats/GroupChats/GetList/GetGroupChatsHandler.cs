using ChatZone.Shared.Context;
using ChatZone.Shared.DTOs;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ChatZone.Features.Chats.GroupChats.GetList;

public class GetGroupChatsHandler(
    ChatZoneDbContext dbContext) : IRequestHandler<GetGroupChatsRequest, Result<List<GetGroupChatsResponse>>>
{
    public async Task<Result<List<GetGroupChatsResponse>>> Handle(GetGroupChatsRequest request, CancellationToken cancellationToken)
    {
        return Result<List<GetGroupChatsResponse>>.Ok(
            await dbContext.GroupChats
                .AsNoTracking()
                .Where(x=> x.BlockedGroupMembers.All(q => q.IdBlockedPerson != request.IdPerson))
                .Select(x => new GetGroupChatsResponse
                {
                    IdGroup = x.IdGroupChat,
                    Title = x.Title,
                    Country = x.Country,
                    City = x.City,
                    Age = x.Age,
                    Lang = x.Lang,
                    PersonCount = x.UserCount
                }).ToListAsync(cancellationToken));
    }
}