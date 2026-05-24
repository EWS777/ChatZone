using ChatZone.Core.Extensions.Exceptions;
using ChatZone.Shared.Context;
using ChatZone.Shared.DTOs;
using ChatZone.Shared.Hubs;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace ChatZone.Features.GroupMembers.ChangeAdmin;

public class ChangeAdminHandler(
    ChatZoneDbContext dbContext,
    IHubContext<ChatZoneHub> hubContext) : IRequestHandler<ChangeAdminRequest, Result<bool>>
{
    public async Task<Result<bool>> Handle(ChangeAdminRequest request, CancellationToken cancellationToken)
    {
        var currentAdmin = await dbContext.GroupMembers.SingleOrDefaultAsync(x => x.IdChat == request.IdGroup && x.IdGroupMember == request.IdPerson && x.IsAdmin == true, cancellationToken);
        if(currentAdmin is null) return Result<bool>.Failure(new ForbiddenAccessException("You are not exist in this group or you are not an admin!"));

        var newAdminPerson = await dbContext.GroupMembers.SingleOrDefaultAsync(x => x.IdChat == request.IdGroup && x.IdGroupMember == request.IdNewAdminPerson && x.IsAdmin == false, cancellationToken);
        if(newAdminPerson is null) return Result<bool>.Failure(new ForbiddenAccessException("You are not exist in this group or you are an admin already!"));

        currentAdmin.IsAdmin = false;
        newAdminPerson.IsAdmin = true;

        dbContext.GroupMembers.Update(currentAdmin);
        dbContext.GroupMembers.Update(newAdminPerson);
        await dbContext.SaveChangesAsync(cancellationToken);

        await hubContext.Clients.User(request.IdPerson.ToString()).SendAsync("AdminChanged", false, cancellationToken);
        await hubContext.Clients.User(request.IdNewAdminPerson.ToString()).SendAsync("AdminChanged", true, cancellationToken);
        
        return Result<bool>.Ok(true);
    }
}