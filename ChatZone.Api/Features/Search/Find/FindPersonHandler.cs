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
    : IRequestHandler<FindPersonRequest, Result<bool>>
{
    public async Task<Result<bool>> Handle(FindPersonRequest request, CancellationToken cancellationToken)
    {
        var matchResult = await matchmakingService.FindMatch(request, cancellationToken);
        
        if(!matchResult.IsSuccess) return Result<bool>.Failure(matchResult.Exception);
        
        if(matchResult.Value == null) return Result<bool>.Ok(false);

        var (person1, person2, idGroup) = matchResult.Value.Value;
        await hubContext.Groups.AddToGroupAsync(person1.ConnectionId, idGroup.ToString(), cancellationToken);
        await hubContext.Groups.AddToGroupAsync(person2.ConnectionId, idGroup.ToString(), cancellationToken);

        await hubContext.Clients.Group(idGroup.ToString()).SendAsync("ChatCreated", cancellationToken);
        return Result<bool>.Ok(true);
    }
}