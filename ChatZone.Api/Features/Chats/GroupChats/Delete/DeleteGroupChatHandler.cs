using ChatZone.Core.Extensions.Exceptions;
using ChatZone.Shared.Context;
using ChatZone.Shared.DTOs;
using ChatZone.Shared.Hubs;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace ChatZone.Features.Chats.GroupChats.Delete;

public class DeleteGroupChatHandler(
    ChatZoneDbContext dbContext,
    IHubContext<ChatZoneHub> hubContext) : IRequestHandler<DeleteGroupChatRequest, Result<IActionResult>>
{
    public async Task<Result<IActionResult>> Handle(DeleteGroupChatRequest request, CancellationToken cancellationToken)
    {
        var isAdmin = await dbContext.GroupMembers.AnyAsync(
            x => x.IdGroupMember == request.IdPerson && x.IdChat == request.IdGroup && x.IsAdmin == true,
            cancellationToken);
        if(!isAdmin) return Result<IActionResult>.Failure(new ForbiddenAccessException("You are not an owner of this group!"));

        await dbContext.GroupMembers
            .Where(x=>x.IdChat == request.IdGroup)
            .ExecuteDeleteAsync(cancellationToken);
        
        await dbContext.BlockedGroupMembers
            .Where(x=>x.IdChat == request.IdGroup)
            .ExecuteDeleteAsync(cancellationToken);

        var group = await dbContext.GroupChats.SingleOrDefaultAsync(x => x.IdGroupChat == request.IdGroup,
            cancellationToken);
        
        dbContext.GroupChats.Remove(group!);
        await dbContext.SaveChangesAsync(cancellationToken);
        
        await hubContext.Clients.Group(request.IdGroup.ToString()).SendAsync("NotifyDeleteGroup", cancellationToken);
        return Result<IActionResult>.Ok(new OkObjectResult(new {message = "Group was deleted successfully!"}));
    }
}