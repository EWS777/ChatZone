using ChatZone.Context;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ChatZone.Features.Chats.Common.GetChat;

public class GetChatHandler(
    ChatZoneDbContext dbContext) : IRequestHandler<GetChatRequest, int?>
{
    public async Task<int?> Handle(GetChatRequest request, CancellationToken cancellationToken)
    {
        var singleChat = await dbContext.SingleChats
            .SingleOrDefaultAsync(x=> (x.IdFirstPerson == request.IdPerson || x.IdSecondPerson == request.IdPerson) 
                                      && x.FinishedAt == null, cancellationToken);
        if (singleChat != null) return singleChat.IdSingleChat;
        
        var groupChat = await dbContext.GroupMembers.SingleOrDefaultAsync(x=>x.IdGroupMember == request.IdPerson, cancellationToken);
        return groupChat?.IdChat;
    }
}