using ChatZone.Context;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ChatZone.Features.Chats.SingleChats.Finish;

public class FinishSingleChatHandler(
    ChatZoneDbContext dbContext) : IRequestHandler<FinishSingleChatRequest, Unit>
{
    public async Task<Unit> Handle(FinishSingleChatRequest request, CancellationToken cancellationToken)
    {
        var singleChat = await dbContext.SingleChats.SingleOrDefaultAsync(x=>x.IdSingleChat == request.IdChat, cancellationToken);
        
        singleChat.FinishedAt = DateTimeOffset.UtcNow;

        await dbContext.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }
}