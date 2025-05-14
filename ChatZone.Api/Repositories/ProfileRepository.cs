using ChatZone.Context;
using ChatZone.Core.Extensions;
using ChatZone.Core.Extensions.Exceptions;
using ChatZone.Core.Models;
using ChatZone.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ChatZone.Repositories;

public class ProfileRepository(ChatZoneDbContext dbContext,
                            IAuthRepository authRepository) : IProfileRepository
{
    public async Task<Result<BlockedPerson[]>> GetBlockedPersonsAsync(string username)
    {
        var person = await authRepository.GetPersonByUsernameAsync(username);

        var blockedPersons = dbContext.BlockedPeoples
            .Where(x => x.IdBlockedPerson == person.Value.IdPerson)
            .ToArray();

        return Result<BlockedPerson[]>.Ok(blockedPersons);
    }

    public async Task<Result<IActionResult>> DeleteBlockedPersonAsync(string username, int idBlockedPerson)
    {
        var person = await authRepository.GetPersonByUsernameAsync(username);

        var deletePerson = dbContext.BlockedPeoples.SingleOrDefault(x => x.IdBlockedPerson == person.Value.IdPerson && x.IdBlockerPerson==idBlockedPerson);
        if (deletePerson is null) return Result<IActionResult>.Failure(new NotFoundException("Person is not found!"));

        dbContext.BlockedPeoples.Remove(deletePerson);
        await dbContext.SaveChangesAsync();
        return Result<IActionResult>.Ok(new OkResult());
    }

    public async Task<Result<QuickMessage[]>> GetQuickMessagesAsync(string username)
    {
        var person = await authRepository.GetPersonByUsernameAsync(username);

        var quickMessageList = dbContext.QuickMessages.Where(x => x.IdPerson == person.Value.IdPerson).ToArray();

        return Result<QuickMessage[]>.Ok(quickMessageList);
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

    public async Task<Result<QuickMessage>> CreateQuickMessagesAsync(string username, string message)
    {
        var person = await authRepository.GetPersonByUsernameAsync(username);
        
        var newQuickMessage = new QuickMessage
        {
            Message = message,
            Person = person.Value
        };

        dbContext.QuickMessages.Add(newQuickMessage);

        await dbContext.SaveChangesAsync();
        return Result<QuickMessage>.Ok(newQuickMessage);
    }
}