using ChatZone.Shared.Context;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ChatZone.Features.Chats.Common.GetActiveChat;

public class GetActiveChatHandler(ChatZoneDbContext dbContext) : IRequestHandler<GetActiveChatRequest, GetActiveChatResponse>
{
    public async Task<GetActiveChatResponse> Handle(GetActiveChatRequest request, CancellationToken cancellationToken)
    {
        var singleChatId = await dbContext.SingleChats
            .Where(x => (x.IdFirstPerson == request.IdPerson || x.IdSecondPerson == request.IdPerson) && x.FinishedAt == null)
            .Select(x => (int?)x.IdSingleChat)
            .FirstOrDefaultAsync(cancellationToken);
        
        if (singleChatId.HasValue) return new GetActiveChatResponse { IdChat = singleChatId.Value, IsSingleChat = true };
        
        var groupChatId = await dbContext.GroupMembers
            .Where(x => x.IdGroupMember == request.IdPerson)
            .Select(x => (int?)x.IdChat)
            .FirstOrDefaultAsync(cancellationToken);
        
        if (groupChatId.HasValue) return new GetActiveChatResponse { IdChat = groupChatId.Value, IsSingleChat = false };
        return new GetActiveChatResponse { IdChat = null, IsSingleChat = null };
    }
}