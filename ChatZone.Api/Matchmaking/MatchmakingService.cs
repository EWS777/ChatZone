using ChatZone.Context;
using ChatZone.Core.Models;
using ChatZone.Features.Search.Find;
using Microsoft.EntityFrameworkCore;

namespace ChatZone.Matchmaking;

public class MatchmakingService(ChatZoneDbContext dbContext) : IMatchmakingService
{
    public async Task<(MatchQueue person1, MatchQueue person2, string groupName)?> FindMatch(
        FindPersonRequest request, CancellationToken cancellationToken)
    {
        var firstPerson = new MatchQueue
        {
            IdPerson = request.IdPerson,
            ConnectionId = request.ConnectionId,
            Theme = request.Theme,
            Country = request.Country,
            City = request.City,
            Age = request.Age,
            YourGender = request.YourGender,
            PartnerGender = request.PartnerGender,
            Language = request.Language
        };
        
        var secondPerson = await dbContext.MatchQueues.FirstOrDefaultAsync(x=> x.IdPerson != firstPerson.IdPerson, cancellationToken);
        if (secondPerson == null)
        {
            var isExists = await dbContext.MatchQueues.AnyAsync(x => x.IdPerson == firstPerson.IdPerson, cancellationToken);
            if(!isExists) await dbContext.MatchQueues.AddAsync(firstPerson, cancellationToken);
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
        
        return (firstPerson, secondPerson, chat.IdSingleChat.ToString());
    }
}