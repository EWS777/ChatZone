using ChatZone.Chat;
using ChatZone.Context;
using MediatR;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace ChatZone.Features.GroupMembers.ChangeAdminAuto;

public class ChangeAdminAutoHandler(
    ChatZoneDbContext dbContext,
    IHubContext<ChatHub> hubContext) : IRequestHandler<ChangeAdminAutoRequest, Unit>
{
    public async Task<Unit> Handle(ChangeAdminAutoRequest request, CancellationToken cancellationToken)
    {
        var newAdmin = await dbContext.GroupMembers
            .Where(x => x.IdChat == request.IdChat && x.IdGroupMember != request.IdPersonAdmin)
            .OrderBy(x => x.JoinedAt)
            .FirstOrDefaultAsync(cancellationToken);
        if (newAdmin is null)
        {
            var groupChat = await dbContext.GroupChats.SingleOrDefaultAsync(x => x.IdGroupChat == request.IdChat, cancellationToken);
            dbContext.GroupChats.Remove(groupChat!);
        }
        else
        {
            var previousAdmin = await dbContext.GroupMembers
                .SingleOrDefaultAsync(x=> x.IdChat == request.IdChat && x.IdGroupMember == request.IdPersonAdmin, cancellationToken);

            dbContext.GroupMembers.Remove(previousAdmin!);
            
            var groupChat = await dbContext.GroupChats.SingleOrDefaultAsync(x => x.IdGroupChat == request.IdChat, cancellationToken);
            groupChat!.UserCount -= 1;
            
            newAdmin.IsAdmin = true;
            await hubContext.Clients.User(newAdmin.IdGroupMember.ToString()).SendAsync("ChangeAdminAuto", cancellationToken);
        }

        await dbContext.SaveChangesAsync(cancellationToken);
        
        return Unit.Value;
    }
}