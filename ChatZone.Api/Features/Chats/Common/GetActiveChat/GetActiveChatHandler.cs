using ChatZone.Context;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ChatZone.Features.Chats.Common.GetActiveChat;

public class GetActiveChatHandler(
    ChatZoneDbContext dbContext) : IRequestHandler<GetActiveChatRequest, GetActiveChatResponse>
{
    public async Task<GetActiveChatResponse> Handle(GetActiveChatRequest request, CancellationToken cancellationToken)
    {
        var singleChat = await dbContext.SingleChats
            .SingleOrDefaultAsync(x=> (x.IdFirstPerson == request.IdPerson || x.IdSecondPerson == request.IdPerson) 
                                      && x.FinishedAt == null, cancellationToken);
        if (singleChat != null) return new GetActiveChatResponse { IdChat = singleChat.IdSingleChat, IsSingleChat = true };
        
        var groupChat = await dbContext.GroupMembers.SingleOrDefaultAsync(x=>x.IdGroupMember == request.IdPerson, cancellationToken);

        return groupChat != null
            ? new GetActiveChatResponse { IdChat = groupChat.IdChat, IsSingleChat = false }
            : new GetActiveChatResponse { IdChat = null, IsSingleChat = null };

    }
}