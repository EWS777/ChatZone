using ChatZone.Matchmaking;
using ChatZone.Shared.DTOs;
using ChatZone.Shared.Hubs;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace ChatZone.Features.Search.Find;

public class FindPersonHandler(
    IMatchmakingService matchmakingService,
    IHubContext<ChatZoneHub> hubContext)
    : IRequestHandler<FindPersonRequest, Result<IActionResult>>
{
    public async Task<Result<IActionResult>> Handle(FindPersonRequest request, CancellationToken cancellationToken)
    {
        var match = await matchmakingService.FindMatch(request, cancellationToken);
        
        if(match==null) return Result<IActionResult>.Ok(new ObjectResult(new { message = "Waiting for match...", chatCreated=false}));
        
        await hubContext.Groups.AddToGroupAsync(match.Value.person1.ConnectionId, match.Value.idGroup.ToString(), cancellationToken);
        await hubContext.Groups.AddToGroupAsync(match.Value.person2.ConnectionId, match.Value.idGroup.ToString(), cancellationToken);

        await hubContext.Clients.Group(match.Value.idGroup.ToString())
            .SendAsync("ChatCreated", cancellationToken);
        
        return Result<IActionResult>.Ok(new ObjectResult(new {message = "Chat was created!"}));
    }
}