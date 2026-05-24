using ChatZone.Core.Extensions.Exceptions;
using ChatZone.Core.Models;
using ChatZone.Shared.Context;
using ChatZone.Shared.DTOs;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ChatZone.Features.GroupMembers.Add;

public class AddGroupMemberHandler(ChatZoneDbContext dbContext) : IRequestHandler<AddGroupMemberRequest, Result<bool>>
{
    public async Task<Result<bool>> Handle(AddGroupMemberRequest request, CancellationToken cancellationToken)
    {
        var groupChat = await dbContext.GroupChats.SingleOrDefaultAsync(x => x.IdGroupChat == request.IdGroup, cancellationToken);
        if(groupChat is null) return Result<bool>.Failure(new NotFoundException("Group chat is not found!"));
        
        var isPersonExists = await dbContext.GroupMembers.AnyAsync(x => x.IdChat == request.IdGroup && x.IdGroupMember == request.IdPerson, cancellationToken);
        if(isPersonExists) return Result<bool>.Ok(true);
        
        var isPersonBlocked = await dbContext.BlockedGroupMembers
            .AnyAsync(x=>x.IdChat == request.IdGroup && x.IdBlockedPerson == request.IdPerson, cancellationToken);
        if(isPersonBlocked) return Result<bool>.Failure(new ForbiddenAccessException("You have been blocked in this group!"));
        
        await dbContext.GroupMembers.AddAsync(new GroupMember
        {
            IdChat = request.IdGroup,
            IdGroupMember = request.IdPerson,
            IsAdmin = false,
            JoinedAt = DateTimeOffset.UtcNow
        }, cancellationToken);

        groupChat.UserCount += 1;
        
        await dbContext.SaveChangesAsync(cancellationToken);
        
        return Result<bool>.Ok(true);
    }
}