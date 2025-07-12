using ChatZone.Context;
using ChatZone.Core.Models;
using ChatZone.Features.Search.Find;
using Microsoft.EntityFrameworkCore;

namespace ChatZone.Matchmaking;

public class MatchmakingService(ChatZoneDbContext dbContext) : IMatchmakingService
{
    public async Task<(string connectionPersonId1, string connectionPersonId2, string groupName)?> FindMatch(
        FindPersonRequest request, CancellationToken cancellationToken)
    {
        var firstPerson = new MatchQueue
        {
            IdPerson = request.IdPerson,
            ConnectionId = request.ConnectionId,
            ThemeList = request.ThemeList,
            Country = request.Country,
            City = request.City,
            Age = request.Age,
            Gender = request.Gender,
            Lang = request.Lang
        };
        
        var secondPerson = await dbContext.MatchQueues.FirstOrDefaultAsync(cancellationToken);
        if (secondPerson == null)
        {
            await dbContext.MatchQueues.AddAsync(firstPerson, cancellationToken);
            await dbContext.SaveChangesAsync(cancellationToken);
            return null;
        }
        
        var chat = new SingleChat
        {
            IdFirstPerson = request.IdPerson,
            IdSecondPerson = secondPerson.IdPerson,
            CreatedAt = DateTimeOffset.UtcNow
        };
        
        dbContext.MatchQueues.Remove(secondPerson);
        await dbContext.SingleChats.AddAsync(chat, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);
        
        return (firstPerson.ConnectionId, secondPerson.ConnectionId, chat.IdSingleChat.ToString());
    }
}