using ChatZone.Context;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ChatZone.Features.Chats.Common.GetActiveChat;

public class GetActiveChatHandler(
    ChatZoneDbContext dbContext) : IRequestHandler<GetActiveChatRequest, int?>
{
    public async Task<int?> Handle(GetActiveChatRequest request, CancellationToken cancellationToken)
    {
        var singleChat = await dbContext.SingleChats
            .SingleOrDefaultAsync(x=> (x.IdFirstPerson == request.IdPerson || x.IdSecondPerson == request.IdPerson) 
                                      && x.FinishedAt == null, cancellationToken);
        if (singleChat != null) return singleChat.IdSingleChat;
        
        var groupChat = await dbContext.GroupMembers.SingleOrDefaultAsync(x=>x.IdGroupMember == request.IdPerson, cancellationToken);
        return groupChat?.IdChat;
    }
}