using ChatZone.Core.Extensions.Exceptions;
using ChatZone.Core.Models;
using ChatZone.Shared.Context;
using ChatZone.Shared.DTOs;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ChatZone.Features.GroupMembers.Add;

public class AddGroupMemberHandler(
    ChatZoneDbContext dbContext) : IRequestHandler<AddGroupMemberRequest, Result<IActionResult>>
{
    public async Task<Result<IActionResult>> Handle(AddGroupMemberRequest request, CancellationToken cancellationToken)
    {
        var isPersonBlocked = await dbContext.BlockedGroupMembers
            .AnyAsync(x=>x.IdChat == request.IdGroup && x.IdBlockedPerson == request.IdPerson, cancellationToken);
        if(isPersonBlocked) return Result<IActionResult>.Failure(new ForbiddenAccessException("You have been blocked in this group!"));
        
        await dbContext.GroupMembers.AddAsync(new GroupMember
        {
            IdChat = request.IdGroup,
            IdGroupMember = request.IdPerson,
            IsAdmin = false,
            JoinedAt = DateTimeOffset.Now
        }, cancellationToken);

        var groupChat = await dbContext.GroupChats.SingleOrDefaultAsync(x => x.IdGroupChat == request.IdGroup, cancellationToken);
        if(groupChat is null) return Result<IActionResult>.Failure(new NotFoundException("Group chat is not found!"));

        groupChat.UserCount += 1;
        
        await dbContext.SaveChangesAsync(cancellationToken);
        
        return Result<IActionResult>.Ok(new OkResult());
    }
}