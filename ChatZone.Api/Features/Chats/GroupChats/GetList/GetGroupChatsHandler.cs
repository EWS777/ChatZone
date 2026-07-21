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
        var query = dbContext.GroupChats
            .AsNoTracking()
            .Where(x => x.BlockedGroupMembers.All(q => q.IdBlockedPerson != request.IdPerson));
        
        if (request.Cursor.HasValue) query = query.Where(x => x.CreatedAt < request.Cursor.Value);
        
        var groupChats = await query
            .OrderByDescending(x => x.CreatedAt)
            .Take(request.TakeGroup)
            .Select(x => new GetGroupChatsResponse
            {
                IdGroup = x.IdGroupChat,
                Title = x.Title,
                Country = x.Country,
                City = x.City,
                Age = x.Age,
                Lang = x.Lang,
                PersonCount = x.UserCount,
                CreatedAt = x.CreatedAt
            }).ToListAsync(cancellationToken);
        
        return Result<List<GetGroupChatsResponse>>.Ok(groupChats);
    }
}