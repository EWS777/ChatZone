using ChatZone.Context;
using ChatZone.Core.Extensions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ChatZone.Features.GroupMembers.Delete;

public class DeleteGroupMemberHandler(
    ChatZoneDbContext dbContext) : IRequestHandler<DeleteGroupMemberRequest, Result<IActionResult>>
{
    public async Task<Result<IActionResult>> Handle(DeleteGroupMemberRequest request, CancellationToken cancellationToken)
    {
        var groupMember = await dbContext.GroupMembers.SingleOrDefaultAsync(x => x.IdGroupMember == request.IdPerson, cancellationToken);
        
        dbContext.GroupMembers.Remove(groupMember!);
        await dbContext.SaveChangesAsync(cancellationToken);
        return Result<IActionResult>.Ok(new OkResult());
    }
}