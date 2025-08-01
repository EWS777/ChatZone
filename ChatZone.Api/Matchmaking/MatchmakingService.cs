using ChatZone.Context;
using ChatZone.Core.Models;
using ChatZone.Features.Search.Find;
using Microsoft.EntityFrameworkCore;

namespace ChatZone.Matchmaking;

public class MatchmakingService(ChatZoneDbContext dbContext) : IMatchmakingService
{
    public async Task<(MatchQueue person1, MatchQueue person2, int idGroup)?> FindMatch(
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
        
        if (request.IsSearchAgain)
        {
            var firstPersonAgain = await dbContext.PersonFilterProperties.SingleOrDefaultAsync(x => x.IdPerson == request.IdPerson, cancellationToken);
            
            firstPerson.Theme = firstPersonAgain!.Theme;
            firstPerson.Country = firstPersonAgain.Country;
            firstPerson.City = firstPersonAgain.City;
            firstPerson.Age = firstPersonAgain.Age;
            firstPerson.YourGender = firstPersonAgain.YourGender;
            firstPerson.PartnerGender = firstPersonAgain.PartnerGender;
            firstPerson.Language = firstPersonAgain.Language;
        }
        
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
        
        if (!request.IsSearchAgain)
        {
            var isPersonExists = await dbContext.PersonFilterProperties.AnyAsync(x => x.IdPerson == firstPerson.IdPerson, cancellationToken);
            if (!isPersonExists)
            {
                await AddFilterPropertiesAsync(firstPerson, cancellationToken);
                await AddFilterPropertiesAsync(secondPerson, cancellationToken);
            }
        }
        
        dbContext.MatchQueues.Remove(secondPerson);
        await dbContext.SingleChats.AddAsync(chat, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);
        
        return (firstPerson, secondPerson, chat.IdSingleChat);
    }

    private async Task AddFilterPropertiesAsync(MatchQueue person, CancellationToken cancellationToken)
    { 
        await dbContext.PersonFilterProperties.AddAsync(new PersonFilterProperty 
        {
            IdPerson = person.IdPerson,
            Theme = person.Theme,
            Country = person.Country,
            City = person.City,
            Age = person.Age,
            YourGender = person.YourGender,
            PartnerGender = person.PartnerGender,
            Language = person.Language
        },cancellationToken);
    }
}