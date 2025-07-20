using ChatZone.Chat;
using ChatZone.Context;
using ChatZone.Core.Extensions;
using ChatZone.Matchmaking;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace ChatZone.Features.Search.Find;

public class FindPersonHandler(
    ChatZoneDbContext dbContext,
    IMatchmakingService matchmakingService,
    IHubContext<ChatHub> hubContext)
    : IRequestHandler<FindPersonRequest, Result<IActionResult>>
{
    public async Task<Result<IActionResult>> Handle(FindPersonRequest request, CancellationToken cancellationToken)
    {
        var match = await matchmakingService.FindMatch(request, cancellationToken);
        
        if(match==null) return Result<IActionResult>.Ok(new ObjectResult(new { message = "Waiting for match...", chatCreated=false}));
        
        await hubContext.Groups.AddToGroupAsync(match.Value.person1.ConnectionId, match.Value.groupName, cancellationToken);
        await hubContext.Groups.AddToGroupAsync(match.Value.person2.ConnectionId, match.Value.groupName, cancellationToken);

        var username1 = await dbContext.Persons
            .Where(x => x.IdPerson == match.Value.person1.IdPerson)
            .Select(x => x.Username)
            .SingleOrDefaultAsync(cancellationToken);
        
        var username2 = await dbContext.Persons
            .Where(x => x.IdPerson == match.Value.person2.IdPerson)
            .Select(x => x.Username)
            .SingleOrDefaultAsync(cancellationToken);
        
        ChatHub.UsersGroups[username1!] = match.Value.groupName;
        ChatHub.UsersGroups[username2!] = match.Value.groupName;
        ChatHub.IsTypeOfChatSingle[match.Value.groupName] = true;

        await hubContext.Clients.Group(match.Value.groupName)
            .SendAsync("ChatCreated", cancellationToken);
        
        return Result<IActionResult>.Ok(new ObjectResult(new {message = "Chat was created!"}));
    }
}