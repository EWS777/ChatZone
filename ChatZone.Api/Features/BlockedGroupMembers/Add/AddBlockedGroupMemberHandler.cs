using ChatZone.Chat;
using ChatZone.Context;
using ChatZone.Core.Extensions;
using ChatZone.Core.Extensions.Exceptions;
using ChatZone.Core.Models;
using ChatZone.Core.Services;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace ChatZone.Features.BlockedGroupMembers.Add;

public class AddBlockedGroupMemberHandler(
    ChatZoneDbContext dbContext,
    IHubContext<ChatHub> hubContext) : IRequestHandler<AddBlockedGroupRequest, Result<IActionResult>>
{
    public async Task<Result<IActionResult>> Handle(AddBlockedGroupRequest request, CancellationToken cancellationToken)
    {
        var isAdmin = await dbContext.GroupMembers
            .Where(x => x.IdChat == request.IdChat && x.IdGroupMember == request.IdAdminPerson)
            .Select(x => x.IsAdmin)
            .SingleOrDefaultAsync(cancellationToken);
        if(!isAdmin) return Result<IActionResult>.Failure(new ForbiddenAccessException("You are not an owner of this group!"));

        var groupMember = await dbContext.GroupMembers.SingleOrDefaultAsync(
            x => x.IdGroupMember == request.IdBlockedPerson, cancellationToken);
        if(groupMember is null) return Result<IActionResult>.Failure(new NotFoundException("Person is not found in group!")); 
        
        dbContext.GroupMembers.Remove(groupMember);

        await dbContext.BlockedGroupMembers.AddAsync(new BlockedGroupMember
        {
            IdChat = request.IdChat,
            IdBlockedPerson = request.IdBlockedPerson,
            BlockedAt = DateTimeOffset.UtcNow
        }, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);

        ChatManagerService.RemovePersonFromGroup(request.IdBlockedPerson);
        await hubContext.Clients.User(request.IdBlockedPerson.ToString()).SendAsync("BlockedIntoGroupChat", cancellationToken);
        
        return Result<IActionResult>.Ok(new OkObjectResult(new {message = "Person has blocked successfully!"}));
    }
}