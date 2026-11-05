using ChatZone.Core.Models;
using ChatZone.Core.Models.Enums;
using ChatZone.Features.Search.Find;
using ChatZone.Shared.Context;
using ChatZone.Shared.DTOs;
using Microsoft.EntityFrameworkCore;

namespace ChatZone.Matchmaking;

public class MatchmakingService(ChatZoneDbContext dbContext) : IMatchmakingService
{
    public async Task<Result<MatchResponse>> FindMatch(FindPersonRequest request, CancellationToken cancellationToken)
    {
        var currentPerson = await dbContext.MatchQueues.FirstOrDefaultAsync(x => x.IdPerson == request.IdPerson, cancellationToken);

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
                YourGender = request.YourGender ?? GenderList.Both,
                PartnerGender = request.PartnerGender ?? GenderList.Both,
                Language = request.Language ?? LangList.Random,
                JoinedAt = DateTimeOffset.UtcNow,
                IsRandomPartner = request.IsFindRandomPerson
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
            currentPerson.YourGender = request.YourGender ?? GenderList.Both;
            currentPerson.PartnerGender = request.PartnerGender ?? GenderList.Both;
            currentPerson.Language = request.Language ?? LangList.Random;
            currentPerson.IsRandomPartner = request.IsFindRandomPerson;
        }

        await dbContext.SaveChangesAsync(cancellationToken);

        var partner = await FindBestPartnerAsync(currentPerson, cancellationToken);
        
        if (partner != null)
        {
            await using var transaction = await dbContext.Database.BeginTransactionAsync(cancellationToken);

            try
            {
                var deletedCount = await dbContext.MatchQueues
                    .Where(x => x.IdPerson == partner.IdPerson || x.IdPerson == currentPerson.IdPerson)
                    .ExecuteDeleteAsync(cancellationToken);
                
                if (deletedCount < 2)
                {
                    await transaction.RollbackAsync(cancellationToken);
                    return Result<MatchResponse>.Ok(new MatchResponse { IsFound = false });
                }
                
                var isRandom = currentPerson.IsRandomPartner || partner.IsRandomPartner;
                var newChat = new SingleChat
                {
                    IdFirstPerson = currentPerson.IdPerson,
                    IdSecondPerson = partner.IdPerson,
                    CreatedAt = DateTimeOffset.UtcNow,
                    Theme = isRandom ? null : Common(currentPerson.Theme, partner.Theme),
                    Country = isRandom ? null : Common(currentPerson.Country, partner.Country),
                    City = isRandom ? null : Common(currentPerson.City, partner.City),
                    Age = isRandom ? null : Common(currentPerson.Age, partner.Age),
                    Language = isRandom || currentPerson.Language == LangList.Random || partner.Language == LangList.Random ? null : Common<LangList>(currentPerson.Language, partner.Language)
                };

                await dbContext.SingleChats.AddAsync(newChat, cancellationToken);

                await dbContext.SaveChangesAsync(cancellationToken);
                await transaction.CommitAsync(cancellationToken);

                return Result<MatchResponse>.Ok(new MatchResponse { Person1 = currentPerson, Person2 = partner, IdGroup = newChat.IdSingleChat, IsFound = true });
            }
            catch (Exception)
            {
                await transaction.RollbackAsync(cancellationToken);
                throw;
            }
        }
        
        return Result<MatchResponse>.Ok(new MatchResponse { IsFound = false });
    }

    private static T? Common<T>(T? first, T? second) where T : struct =>
        first.HasValue && EqualityComparer<T?>.Default.Equals(first, second) ? first : null;

    private async Task<MatchQueue?> FindBestPartnerAsync(MatchQueue currentPerson, CancellationToken cancellationToken)
    {
        var secondsWaiting = (DateTimeOffset.UtcNow - currentPerson.JoinedAt).TotalSeconds;
        
        var blockedUsers = await GetListOfBlockedUsersById(currentPerson.IdPerson, cancellationToken);
        
        if (currentPerson.IsRandomPartner)
        {
            return await dbContext.MatchQueues
                .Where(x => x.IdPerson != currentPerson.IdPerson
                            && !blockedUsers.Contains(x.IdPerson)
                            && x.IsRandomPartner)
                .OrderBy(x => x.JoinedAt)
                .FirstOrDefaultAsync(cancellationToken);
        }
        
        //main filter
        var filteredPeople = dbContext.MatchQueues
            .Where(x => x.IdPerson != currentPerson.IdPerson
                        && !blockedUsers.Contains(x.IdPerson) //to eliminate blocked users 
                        && (currentPerson.Language == LangList.Random || x.Language == LangList.Random || x.Language == currentPerson.Language) //Random = any language
                        && !x.IsRandomPartner
                        && (currentPerson.PartnerGender == GenderList.Both || currentPerson.PartnerGender == x.YourGender)
                        && (x.PartnerGender == GenderList.Both || x.PartnerGender == currentPerson.YourGender)
            );

        var scoredQuery = filteredPeople.Select(q => new
        {
            Queue = q,
            Score = (q.Age == currentPerson.Age ? 3 : 0) +
                    (q.Theme == currentPerson.Theme ? 2 : 0) +
                    (q.City == currentPerson.City ? 1 : 0) +
                    (q.Country == currentPerson.Country ? 1 : 0)
        });
        
        var requiredScore = GetRequiredScore(secondsWaiting);
        scoredQuery = scoredQuery.Where(x => x.Score >= requiredScore);

        return await scoredQuery
            .OrderByDescending(x => x.Score)
            .ThenBy(x => x.Queue.JoinedAt)
            .Select(x => x.Queue)
            .FirstOrDefaultAsync(cancellationToken);
    }
    
    private async Task<List<int>> GetListOfBlockedUsersById(int currentPersonId, CancellationToken cancellationToken)
    {
        var fiveMinutesAgo = DateTimeOffset.UtcNow.AddMinutes(-5);
        
        //skipping chats which was created less than 5 minutes and were active less than 30 seconds
        var skippedChats = await dbContext.SingleChats
            .Where(x => (x.IdSecondPerson == currentPersonId || x.IdFirstPerson == currentPersonId) 
                        && x.FinishedAt != null
                        && x.FinishedAt >= fiveMinutesAgo
                        && x.FinishedAt <= x.CreatedAt.AddSeconds(30))
            .Select(x => x.IdFirstPerson == currentPersonId ? x.IdSecondPerson : x.IdFirstPerson)
            .ToListAsync(cancellationToken);
        
        //list of blocked users by us and where someone blocked us
        var blockedUsers = await dbContext.BlockedPeoples
            .Where(x => x.IdBlockerPerson == currentPersonId || x.IdBlockedPerson == currentPersonId)
            .Select(x => x.IdBlockerPerson == currentPersonId ? x.IdBlockedPerson : x.IdBlockerPerson)
            .ToListAsync(cancellationToken);
        
        //add IdPerson from skipped chats
        blockedUsers.AddRange(skippedChats);
        
        return blockedUsers;
    }
    
    private static int GetRequiredScore(double secondsWaiting)
    {
        if (secondsWaiting < 15) return 6; // all
        if (secondsWaiting < 30) return 5; // age + theme
        if (secondsWaiting < 45) return 3; // age
        if (secondsWaiting < 60) return 2; // theme
        
        return 0;
    }
}