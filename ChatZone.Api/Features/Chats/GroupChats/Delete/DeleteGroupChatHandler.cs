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
    IHubContext<ChatZoneHub> hubContext) : IRequestHandler<DeleteGroupChatRequest, Result<bool>>
{
    public async Task<Result<bool>> Handle(DeleteGroupChatRequest request, CancellationToken cancellationToken)
    {
        var isAdmin = await dbContext.GroupMembers.AnyAsync(x => x.IdChat == request.IdGroup && x.IdGroupMember == request.IdPerson && x.IsAdmin == true, cancellationToken);
        if(!isAdmin) return Result<bool>.Failure(new ForbiddenAccessException("You are not an owner of this group!"));
        
        var group = await dbContext.GroupChats.SingleOrDefaultAsync(x => x.IdGroupChat == request.IdGroup, cancellationToken);
        if(group is null) return Result<bool>.Failure(new NotFoundException("Group chat is not found!"));
        
        await using var transaction = await dbContext.Database.BeginTransactionAsync(cancellationToken);
        try
        {
            await dbContext.GroupMembers
                .Where(x=>x.IdChat == request.IdGroup)
                .ExecuteDeleteAsync(cancellationToken);
            
            await dbContext.BlockedGroupMembers
                .Where(x=>x.IdChat == request.IdGroup)
                .ExecuteDeleteAsync(cancellationToken);
            
            await dbContext.GroupMessages
                .Where(x => x.IdChat == request.IdGroup)
                .ExecuteDeleteAsync(cancellationToken);
        
            dbContext.GroupChats.Remove(group);
            
            await dbContext.SaveChangesAsync(cancellationToken);
            await transaction.CommitAsync(cancellationToken);
            
            await hubContext.Clients.Group(request.IdGroup.ToString()).SendAsync("NotifyDeleteGroup", cancellationToken);
            return Result<bool>.Ok(true);
        }
        catch (Exception)
        {
            await transaction.RollbackAsync(cancellationToken);
            return Result<bool>.Failure(new BackendException());
        }
    }
}