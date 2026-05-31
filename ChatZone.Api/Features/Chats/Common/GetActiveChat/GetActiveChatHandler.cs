using ChatZone.Shared.Context;
using ChatZone.Shared.DTOs;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ChatZone.Features.Chats.Common.GetActiveChat;

public class GetActiveChatHandler(ChatZoneDbContext dbContext) : IRequestHandler<GetActiveChatRequest, Result<GetActiveChatResponse>>
{
    public async Task<Result<GetActiveChatResponse>> Handle(GetActiveChatRequest request, CancellationToken cancellationToken)
    {
        var singleChatId = await dbContext.SingleChats
            .AsNoTracking()
            .Where(x => (x.IdFirstPerson == request.IdPerson || x.IdSecondPerson == request.IdPerson) && x.FinishedAt == null)
            .Select(x => (int?)x.IdSingleChat)
            .SingleOrDefaultAsync(cancellationToken);
        
        if (singleChatId.HasValue) return Result<GetActiveChatResponse>.Ok(new GetActiveChatResponse{ IdChat = singleChatId.Value, IsSingleChat = true });
        
        var groupChatId = await dbContext.GroupMembers
            .AsNoTracking()
            .Where(x => x.IdGroupMember == request.IdPerson)
            .Select(x => (int?)x.IdChat)
            .SingleOrDefaultAsync(cancellationToken);
        
        if (groupChatId.HasValue) return Result<GetActiveChatResponse>.Ok(new GetActiveChatResponse { IdChat = groupChatId.Value, IsSingleChat = false });
        return Result<GetActiveChatResponse>.Ok(new GetActiveChatResponse { IdChat = null, IsSingleChat = null } );
    }
}