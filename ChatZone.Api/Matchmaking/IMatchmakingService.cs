using ChatZone.Features.Search.Find;

namespace ChatZone.Matchmaking;

public interface IMatchmakingService
{
    Task<(string connectionPersonId1, string connectionPersonId2, string groupName)?> FindMatch(
        FindPersonRequest request, CancellationToken cancellationToken);
}