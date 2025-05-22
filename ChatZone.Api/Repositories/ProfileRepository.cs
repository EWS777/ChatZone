using ChatZone.Context;
using ChatZone.Core.Extensions;
using ChatZone.Core.Extensions.Exceptions;
using ChatZone.Core.Models;
using ChatZone.DTO.Responses;
using ChatZone.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ChatZone.Repositories;

public class ProfileRepository(ChatZoneDbContext dbContext,
                            IAuthRepository authRepository) : IProfileRepository
{
    public async Task<Result<BlockedPersonResponse[]>> GetBlockedPersonsAsync(string username)
    {
        var person = await authRepository.GetPersonByUsernameAsync(username);

        var blockedPersons = await dbContext.BlockedPeoples
            .Where(x => x.IdBlockerPerson == person.Value.IdPerson)
            .Select(x=>new BlockedPersonResponse
            {
                IdBlockedPerson = x.IdBlockedPerson,
                BlockedUsername = x.Blocked.Username,
                CreatedAt = x.CreatedAt
            })
            .ToArrayAsync();

        return Result<BlockedPersonResponse[]>.Ok(blockedPersons);
    }

    public async Task<Result<IActionResult>> DeleteBlockedPersonAsync(string username, int idBlockedPerson)
    {
        var person = await authRepository.GetPersonByUsernameAsync(username);
        var deletePerson = dbContext.BlockedPeoples.SingleOrDefault(x => x.IdBlockerPerson == person.Value.IdPerson && x.IdBlockedPerson == idBlockedPerson);
        if (deletePerson is null) return Result<IActionResult>.Failure(new NotFoundException("Person is not found!"));

        dbContext.BlockedPeoples.Remove(deletePerson);
        await dbContext.SaveChangesAsync();
        return Result<IActionResult>.Ok(new OkResult());
    }

    public async Task<Result<QuickMessageResponse[]>> GetQuickMessagesAsync(string username)
    {
        var person = await authRepository.GetPersonByUsernameAsync(username);

        var quickMessageList = await dbContext.QuickMessages
            .Where(x => x.IdPerson == person.Value.IdPerson)
            .Select(x=>new QuickMessageResponse
            {
                IdQuickMessage = x.IdQuickMessage,
                Message = x.Message
            })
            .ToArrayAsync();

        return Result<QuickMessageResponse[]>.Ok(quickMessageList);
    }

    public async Task<Result<QuickMessage>> GetQuickMessageAsync(int idPerson, int idQuickMessage)
    {
        var quickMessage =
            await dbContext.QuickMessages.SingleOrDefaultAsync(x => x.IdPerson == idPerson
                                                         && x.IdQuickMessage == idQuickMessage);

        if (quickMessage is null)
            return Result<QuickMessage>.Failure(new NotFoundException("Quick message is not found!"));
        
        return Result<QuickMessage>.Ok(quickMessage);
    }

    public async Task<Result<QuickMessageResponse>> CreateQuickMessagesAsync(string username, string message)
    {
        var person = await authRepository.GetPersonByUsernameAsync(username);

        var quickMessage = new QuickMessage
        {
            IdPerson = person.Value.IdPerson,
            Message = message
        };
        dbContext.QuickMessages.Add(quickMessage);


        await dbContext.SaveChangesAsync();
        return Result<QuickMessageResponse>.Ok(new QuickMessageResponse
        {
            IdQuickMessage = quickMessage.IdQuickMessage,
            Message = message
        });
    }
}