using ChatZone.Core.Extensions.Exceptions;
using ChatZone.Shared.Context;
using ChatZone.Shared.DTOs;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ChatZone.Features.GroupMembers.LeaveGroup;

public class LeaveGroupHandler(ChatZoneDbContext dbContext) : IRequestHandler<LeaveGroupRequest, Result<bool>>
{
    public async Task<Result<bool>> Handle(LeaveGroupRequest request, CancellationToken cancellationToken)
    {
        var groupMember = await dbContext.GroupMembers
            .Include(x=>x.GroupChat)
            .SingleOrDefaultAsync(x => x.IdChat == request.IdChat && x.IdGroupMember == request.IdPerson, cancellationToken);
        if(groupMember is null) return Result<bool>.Failure(new NotFoundException("You are not a member of this group!"));
        
        groupMember.GroupChat.UserCount -= 1;
        
        dbContext.GroupMembers.Remove(groupMember);
        await dbContext.SaveChangesAsync(cancellationToken);
        return Result<bool>.Ok(true);
    }
}