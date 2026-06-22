using ChatZone.Core.Models;
using ChatZone.Features.Search.Find;
using ChatZone.Shared.DTOs;

namespace ChatZone.Matchmaking;

public interface IMatchmakingService
{
    Task<Result<MatchResponse>> FindMatch(FindPersonRequest request, CancellationToken cancellationToken);
}