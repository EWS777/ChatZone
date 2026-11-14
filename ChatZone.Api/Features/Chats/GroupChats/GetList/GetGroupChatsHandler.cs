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
        
        if (request.Cursor.HasValue)
        {
            var cursorId = request.CursorId ?? int.MaxValue;
            query = query.Where(x => x.CreatedAt < request.Cursor.Value || (x.CreatedAt == request.Cursor.Value && x.IdGroupChat < cursorId));
        }

        var groupChats = await query
            .OrderByDescending(x => x.CreatedAt)
            .ThenByDescending(x => x.IdGroupChat)
            .Take(request.TakeGroup)
            .Select(x => new GetGroupChatsResponse
            {
                IdGroup = x.IdGroupChat,
                Title = x.Title,
                Country = x.Country == null ? null : new NamedItemDto { Id = x.Country.IdCountry, Name = x.Country.Name },
                City = x.City == null ? null : new NamedItemDto { Id = x.City.IdCity, Name = x.City.Name },
                Age = x.Age,
                Language = x.Language == null ? null : new NamedItemDto { Id = x.Language.IdLanguage, Name = x.Language.Name },
                PersonCount = x.UserCount,
                CreatedAt = x.CreatedAt
            }).ToListAsync(cancellationToken);
        
        return Result<List<GetGroupChatsResponse>>.Ok(groupChats);
    }
}