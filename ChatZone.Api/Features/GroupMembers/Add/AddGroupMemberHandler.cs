using ChatZone.Context;
using ChatZone.Core.Extensions;
using ChatZone.Core.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ChatZone.Features.GroupMembers.Add;

public class AddGroupMemberHandler(
    ChatZoneDbContext dbContext) : IRequestHandler<AddGroupMemberRequest, Result<IActionResult>>
{
    public async Task<Result<IActionResult>> Handle(AddGroupMemberRequest request, CancellationToken cancellationToken)
    {
        await dbContext.GroupMembers.AddAsync(new GroupMember
        {
            IdChat = request.IdGroup,
            IdGroupMember = request.IdPerson,
            IsAdmin = false,
            JoinedAt = DateTimeOffset.Now
        }, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);
        
        return Result<IActionResult>.Ok(new OkResult());
    }
}