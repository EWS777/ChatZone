using ChatZone.Core.Extensions.Exceptions;
using ChatZone.Shared.Context;
using ChatZone.Shared.DTOs;
using ChatZone.Shared.Hubs;
using MediatR;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace ChatZone.Features.Chats.SingleChats.Finish;

public class FinishSingleChatHandler(
    ChatZoneDbContext dbContext,
    IHubContext<ChatZoneHub> hubContext) : IRequestHandler<FinishSingleChatRequest, Result<bool>>
{
    public async Task<Result<bool>> Handle(FinishSingleChatRequest request, CancellationToken cancellationToken)
    {
        var singleChat = await dbContext.SingleChats.FirstOrDefaultAsync(x=>x.IdSingleChat == request.IdChat && (x.IdFirstPerson == request.IdPerson || x.IdSecondPerson == request.IdPerson), cancellationToken);
        if(singleChat is null) return Result<bool>.Failure(new NotFoundException("Chat is not found or you don't have access!"));
        
        singleChat.FinishedAt = DateTimeOffset.UtcNow;

        await dbContext.SaveChangesAsync(cancellationToken);
        await hubContext.Clients.Group(request.IdChat.ToString()).SendAsync("LeftChat", cancellationToken);
        return Result<bool>.Ok(true);
    }
}