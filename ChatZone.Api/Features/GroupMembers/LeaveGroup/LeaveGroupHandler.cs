using ChatZone.Context;
using ChatZone.Core.Extensions;
using ChatZone.Core.Extensions.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ChatZone.Features.GroupMembers.LeaveGroup;

public class LeaveGroupHandler(
    ChatZoneDbContext dbContext) : IRequestHandler<LeaveGroupRequest, Result<IActionResult>>
{
    public async Task<Result<IActionResult>> Handle(LeaveGroupRequest request, CancellationToken cancellationToken)
    {
        var groupMember = await dbContext.GroupMembers.SingleOrDefaultAsync(x => x.IdGroupMember == request.IdPerson, cancellationToken);
        
        var groupChat = await dbContext.GroupChats.SingleOrDefaultAsync(x => x.IdGroupChat == request.IdChat, cancellationToken);
        if(groupChat is null) return Result<IActionResult>.Failure(new NotFoundException("Group chat is not found!"));
        groupChat.UserCount -= 1;
        
        dbContext.GroupMembers.Remove(groupMember!);
        await dbContext.SaveChangesAsync(cancellationToken);
        return Result<IActionResult>.Ok(new OkResult());
    }
}