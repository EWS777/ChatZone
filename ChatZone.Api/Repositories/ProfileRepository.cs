using ChatZone.Context;
using ChatZone.Core.Extensions;
using ChatZone.Core.Extensions.Exceptions;
using ChatZone.Core.Models;
using ChatZone.DTO.Responses;
using ChatZone.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ChatZone.Repositories;

public class ProfileRepository(ChatZoneDbContext dbContext) : IProfileRepository
{ 
    public async Task<Result<List<BlockedPersonResponse>>> GetBlockedPersonsAsync(int id)
    {
        var blockedPersons = await dbContext.BlockedPeoples
            .Where(x => x.IdBlockerPerson == id)
            .Select(x => new BlockedPersonResponse
            {
                IdBlockedPerson = x.IdBlockedPerson,
                BlockedUsername = x.Blocked.Username,
                CreatedAt = x.CreatedAt
            })
            .ToListAsync();
        return Result<List<BlockedPersonResponse>>.Ok(blockedPersons);
    }
    
    public async Task<Result<List<QuickMessageResponse>>> GetQuickMessagesAsync(int id)
    {
        var quickMessageList = await dbContext.QuickMessages
            .Where(x => x.IdPerson == id)
            .Select(x => new QuickMessageResponse
            {
                IdQuickMessage = x.IdQuickMessage,
                Message = x.Message
            })
            .ToListAsync();

        return Result<List<QuickMessageResponse>>.Ok(quickMessageList);
    }

    public async Task<Result<QuickMessageResponse>> AddQuickMessageAsync(int id, string message)
    {
        var quickMessage = new QuickMessage
        {
            IdPerson = id,
            Message = message
        };
     
        await dbContext.QuickMessages.AddAsync(quickMessage);
        await dbContext.SaveChangesAsync();
        
        return Result<QuickMessageResponse>.Ok(new QuickMessageResponse
        {
            IdQuickMessage = quickMessage.IdQuickMessage,
            Message = message
        });
    }

    public async Task<Result<IActionResult>> DeleteBlockedPersonAsync(int id, int idBlockedPerson)
    {
        var deletePerson = await dbContext.BlockedPeoples.SingleOrDefaultAsync(x => x.IdBlockerPerson == id && x.IdBlockedPerson == idBlockedPerson);
        if (deletePerson is null) return Result<IActionResult>.Failure(new NotFoundException("Person is not found!"));

        dbContext.BlockedPeoples.Remove(deletePerson);
        await dbContext.SaveChangesAsync();
        
        return Result<IActionResult>.Ok(new OkObjectResult("User was deleted successfully!"));
    }
}