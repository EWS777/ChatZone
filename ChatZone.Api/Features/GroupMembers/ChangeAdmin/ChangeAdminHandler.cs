using ChatZone.Chat;
using ChatZone.Context;
using ChatZone.Core.Extensions;
using ChatZone.Core.Extensions.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace ChatZone.Features.GroupMembers.ChangeAdmin;

public class ChangeAdminHandler(
    ChatZoneDbContext dbContext,
    IHubContext<ChatHub> hubContext) : IRequestHandler<ChangeAdminRequest, Result<IActionResult>>
{
    public async Task<Result<IActionResult>> Handle(ChangeAdminRequest request, CancellationToken cancellationToken)
    {
        var isAdmin = await dbContext.GroupMembers
            .Where(x => x.IdChat == request.IdGroup && x.IdGroupMember == request.IdPerson)
            .SingleOrDefaultAsync(cancellationToken);
        if(isAdmin is null) return Result<IActionResult>.Failure(new ForbiddenAccessException("You are not an owner of this group!"));

        isAdmin.IsAdmin = false;

        var newAdmin = await dbContext.GroupMembers.SingleOrDefaultAsync(
            x => x.IdGroupMember == request.IdNewAdminPerson && x.IdChat == request.IdGroup, cancellationToken);
        if(newAdmin is null) return Result<IActionResult>.Failure(new NotFoundException("New admin is not exists!"));

        newAdmin.IsAdmin = true;

        dbContext.GroupMembers.Update(isAdmin);
        dbContext.GroupMembers.Update(newAdmin);
        await dbContext.SaveChangesAsync(cancellationToken);

        await hubContext.Clients.User(request.IdPerson.ToString()).SendAsync("AdminChanged", false, cancellationToken);
        await hubContext.Clients.User(request.IdNewAdminPerson.ToString()).SendAsync("AdminChanged", true, cancellationToken);
        
        return Result<IActionResult>.Ok(new OkResult());
    }
}