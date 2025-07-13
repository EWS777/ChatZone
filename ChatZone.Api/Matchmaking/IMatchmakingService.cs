using ChatZone.Core.Models;
using ChatZone.Features.Search.Find;

namespace ChatZone.Matchmaking;

public interface IMatchmakingService
{
    Task<(MatchQueue person1, MatchQueue person2, string groupName)?> FindMatch(
        FindPersonRequest request, CancellationToken cancellationToken);
}