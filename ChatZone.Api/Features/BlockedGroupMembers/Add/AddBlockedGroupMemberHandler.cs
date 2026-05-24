using ChatZone.Core.Extensions.Exceptions;
using ChatZone.Core.Models;
using ChatZone.Shared.Context;
using ChatZone.Shared.DTOs;
using ChatZone.Shared.Hubs;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace ChatZone.Features.BlockedGroupMembers.Add;

public class AddBlockedGroupMemberHandler(
    ChatZoneDbContext dbContext,
    IHubContext<ChatZoneHub> hubContext) : IRequestHandler<AddBlockedGroupRequest, Result<bool>>
{
    public async Task<Result<bool>> Handle(AddBlockedGroupRequest request, CancellationToken cancellationToken)
    {
        var isAdmin = await dbContext.GroupMembers.SingleOrDefaultAsync(x => x.IdChat == request.IdChat && x.IdGroupMember == request.IdAdminPerson && x.IsAdmin == true, cancellationToken);
        if(isAdmin is null) return Result<bool>.Failure(new ForbiddenAccessException("You are not an owner of this group!"));

        var groupMember = await dbContext.GroupMembers
            .Include(x=>x.GroupChat)
            .SingleOrDefaultAsync(x=>x.IdChat == request.IdChat && x.IdGroupMember == request.IdBlockedPerson, cancellationToken);
        if(groupMember is null) return Result<bool>.Failure(new NotFoundException("Person is not found in group!")); 
        
        dbContext.GroupMembers.Remove(groupMember);
        groupMember.GroupChat.UserCount -= 1;

        await dbContext.BlockedGroupMembers.AddAsync(new BlockedGroupMember
        {
            IdChat = request.IdChat,
            IdBlockedPerson = request.IdBlockedPerson,
            BlockedAt = DateTimeOffset.UtcNow
        }, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);

        await hubContext.Clients.User(request.IdBlockedPerson.ToString()).SendAsync("BlockedIntoGroupChat", cancellationToken);
        
        return Result<bool>.Ok(true);
    }
}