using ChatZone.Shared.Context;
using ChatZone.Shared.Hubs;
using MediatR;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace ChatZone.Features.Chats.SingleChats.Finish;

public class FinishSingleChatHandler(
    ChatZoneDbContext dbContext,
    IHubContext<ChatZoneHub> hubContext) : IRequestHandler<FinishSingleChatRequest, Unit>
{
    public async Task<Unit> Handle(FinishSingleChatRequest request, CancellationToken cancellationToken)
    {
        var singleChat = await dbContext.SingleChats.SingleOrDefaultAsync(x=>x.IdSingleChat == request.IdChat, cancellationToken);
        
        singleChat.FinishedAt = DateTimeOffset.UtcNow;

        await dbContext.SaveChangesAsync(cancellationToken);
        await hubContext.Clients.Group(request.IdChat.ToString()).SendAsync("LeftChat", cancellationToken);
        return Unit.Value;
    }
}