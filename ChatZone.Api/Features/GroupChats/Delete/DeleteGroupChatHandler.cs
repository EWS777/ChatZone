using ChatZone.Context;
using ChatZone.Core.Extensions;
using ChatZone.Core.Extensions.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ChatZone.Features.GroupChats.Delete;

public class DeleteGroupChatHandler(
    ChatZoneDbContext dbContext) : IRequestHandler<DeleteGroupChatRequest, Result<IActionResult>>
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

        var group = await dbContext.GroupChats.SingleOrDefaultAsync(x => x.IdGroupChat == request.IdGroup,
            cancellationToken);
        
        dbContext.GroupChats.Remove(group!);
        await dbContext.SaveChangesAsync(cancellationToken);
        
        return Result<IActionResult>.Ok(new OkObjectResult(new {message = "Group was deleted successfully!"}));
    }
}