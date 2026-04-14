using ChatZone.Core.Models;
using ChatZone.Core.Models.Enums;
using ChatZone.Features.Search.Find;
using ChatZone.Shared.Context;
using Microsoft.EntityFrameworkCore;

namespace ChatZone.Matchmaking;

public class MatchmakingService(ChatZoneDbContext dbContext) : IMatchmakingService
{
    public async Task<(MatchQueue person1, MatchQueue person2, int idGroup)?> FindMatch(FindPersonRequest request, CancellationToken cancellationToken)
    {
        if(request.YourGender == null || request.PartnerGender == null || request.Language == null) throw new Exception("GenderRequired");

        await using var transaction = await dbContext.Database.BeginTransactionAsync(cancellationToken);

        try
        {
            var currentPerson =
                await dbContext.MatchQueues.FirstOrDefaultAsync(x => x.IdPerson == request.IdPerson, cancellationToken);

            if (currentPerson == null)
            {
                currentPerson = new MatchQueue
                {
                    IdPerson = request.IdPerson,
                    ConnectionId = request.ConnectionId,
                    Theme = request.Theme,
                    Country = request.Country,
                    City = request.City,
                    Age = request.Age,
                    YourGender = request.YourGender.Value,
                    PartnerGender = request.PartnerGender.Value,
                    Language = request.Language.Value,
                    JoinedAt = DateTimeOffset.UtcNow,
                    IsRandomPartner = request.IsRandomPartner
                };
                dbContext.MatchQueues.Add(currentPerson);
            }
            else
            {
                currentPerson.ConnectionId = request.ConnectionId;
                currentPerson.Theme = request.Theme;
                currentPerson.Country = request.Country;
                currentPerson.City = request.City;
                currentPerson.Age = request.Age;
                currentPerson.YourGender = request.YourGender.Value;
                currentPerson.PartnerGender = request.PartnerGender.Value;
                currentPerson.Language = request.Language.Value;
                currentPerson.JoinedAt = DateTimeOffset.UtcNow;
                currentPerson.IsRandomPartner = request.IsRandomPartner;
            }

            await dbContext.SaveChangesAsync(cancellationToken);

            var partner = await FindBestPartnerAsync(currentPerson, cancellationToken);
            
            if (partner != null)
            {
                var newChat = new SingleChat
                {
                    IdFirstPerson = currentPerson.IdPerson,
                    IdSecondPerson = partner.IdPerson,
                    CreatedAt = DateTimeOffset.UtcNow
                };

                await dbContext.SingleChats.AddAsync(newChat, cancellationToken);

                dbContext.MatchQueues.Remove(currentPerson);
                dbContext.MatchQueues.Remove(partner);

                await dbContext.SaveChangesAsync(cancellationToken);
                await transaction.CommitAsync(cancellationToken);

                return (currentPerson, partner, newChat.IdSingleChat);
            }

            await transaction.CommitAsync(cancellationToken);
            return null;
        }
        catch (Exception)
        {
            await transaction.RollbackAsync(cancellationToken);
            throw;
        }
    }

    private async Task<MatchQueue?> FindBestPartnerAsync(MatchQueue currentPerson, CancellationToken cancellationToken)
    {
        var secondsWaiting = (DateTimeOffset.UtcNow - currentPerson.JoinedAt).TotalSeconds;
        var fiveMinutesAgo = DateTimeOffset.UtcNow.AddMinutes(-5);
        
        //skipping chats which was created less than 5 minutes and were active less than 30 seconds
        var skippedChats = await dbContext.SingleChats
            .Where(x => (x.IdSecondPerson == currentPerson.IdPerson || x.IdFirstPerson == currentPerson.IdPerson)
                && x.FinishedAt != null
                && x.FinishedAt >= fiveMinutesAgo
                && x.FinishedAt <= x.CreatedAt.AddSeconds(30))
            .Select(x => x.IdFirstPerson == currentPerson.IdPerson ? x.IdSecondPerson : x.IdFirstPerson)
            .ToListAsync(cancellationToken);
        
        //list of blocked users by us and where someone blocked us
        var blockedUsers = await dbContext.BlockedPeoples
            .Where(x => x.IdBlockerPerson == currentPerson.IdPerson || x.IdBlockedPerson == currentPerson.IdPerson)
            .Select(x=>x.IdBlockerPerson == currentPerson.IdPerson ? x.IdBlockedPerson : x.IdBlockerPerson)
            .ToListAsync(cancellationToken);
        
        //add IdPerson from skipped chats
        blockedUsers.AddRange(skippedChats);

        if (currentPerson.IsRandomPartner)
        {
            return await dbContext.MatchQueues
                .Where(x => x.IdPerson != currentPerson.IdPerson && 
                            !blockedUsers.Contains(x.IdPerson)
                            && x.Language == currentPerson.Language &&
                            x.IsRandomPartner == true)
                .OrderBy(x => x.JoinedAt)
                .FirstOrDefaultAsync(cancellationToken);
        }
        
        //main filter
        var filteredPeople = dbContext.MatchQueues
            .Where(x => x.IdPerson != currentPerson.IdPerson
                    && !blockedUsers.Contains(x.IdPerson)  //to eliminate blocked users 
                    && x.Language == currentPerson.Language 
                    && x.IsRandomPartner == false
                    &&
                     (
                         //me && partner is concrete
                         (currentPerson.PartnerGender == x.YourGender && x.PartnerGender == currentPerson.YourGender)
                         ||
                         //no matter for me && partner is concrete
                         (currentPerson.PartnerGender == GenderList.Both && x.PartnerGender == currentPerson.YourGender)
                         ||
                         //I'm concrete && no matter for partner
                         (currentPerson.PartnerGender == x.YourGender && x.PartnerGender == GenderList.Both)
                         ||
                         //we are both no matter
                         (currentPerson.PartnerGender == GenderList.Both && x.PartnerGender == GenderList.Both)
                     )
            );

        var scoredQuery = filteredPeople.Select(q => new
        {
            Queue = q,
            Score = (q.Age == currentPerson.Age ? 3 : 0) +
                    (q.Theme == currentPerson.Theme ? 2 : 0) +
                    (q.City == currentPerson.City ? 1 : 0) +
                    (q.Country == currentPerson.Country ? 1 : 0)
        });
        
        if (secondsWaiting < 15) scoredQuery = scoredQuery.Where(x => x.Score >= 6); //all
        else if (secondsWaiting < 30) scoredQuery =  scoredQuery.Where(x => x.Score >= 5); //age + theme
        else if (secondsWaiting < 45) scoredQuery =  scoredQuery.Where(x => x.Score >= 3); //age
        else if (secondsWaiting < 60) scoredQuery =  scoredQuery.Where(x => x.Score >= 2); //theme

        return await scoredQuery
            .OrderByDescending(x => x.Score)
            .ThenBy(x => x.Queue.JoinedAt)
            .Select(x => x.Queue)
            .FirstOrDefaultAsync(cancellationToken);
    }
}