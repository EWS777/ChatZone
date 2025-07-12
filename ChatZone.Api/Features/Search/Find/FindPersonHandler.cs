using ChatZone.Chat;
using ChatZone.Core.Extensions;
using ChatZone.Matchmaking;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace ChatZone.Features.Search.Find;

public class FindPersonHandler(
    IMatchmakingService matchmakingService,
    IHubContext<ChatHub> hubContext)
    : IRequestHandler<FindPersonRequest, Result<IActionResult>>
{
    public async Task<Result<IActionResult>> Handle(FindPersonRequest request, CancellationToken cancellationToken)
    {
        var match = await matchmakingService.FindMatch(request, cancellationToken);
        
        if(match==null) return Result<IActionResult>.Ok(new ObjectResult(new { message = "Waiting for match...", chatCreated=false}));
        
        await hubContext.Groups.AddToGroupAsync(match.Value.connectionPersonId1, match.Value.groupName, cancellationToken);
        await hubContext.Groups.AddToGroupAsync(match.Value.connectionPersonId2, match.Value.groupName, cancellationToken);

        await hubContext.Clients.Group(match.Value.groupName)
            .SendAsync("ChatCreated", match.Value.groupName, "Person has found", cancellationToken);
        
        return Result<IActionResult>.Ok(new ObjectResult(new {message = "Chat was created!", chatCreated=true, groupName=match.Value.groupName}));
    }
}