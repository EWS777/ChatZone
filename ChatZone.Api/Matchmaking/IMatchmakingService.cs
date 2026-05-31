using ChatZone.Core.Models;
using ChatZone.Features.Search.Find;
using ChatZone.Shared.DTOs;

namespace ChatZone.Matchmaking;

public interface IMatchmakingService
{
    Task<Result<(MatchQueue person1, MatchQueue person2, int idGroup)?>> FindMatch(FindPersonRequest request, CancellationToken cancellationToken);
}