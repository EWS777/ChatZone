using ChatZone.Core.Extensions;
using ChatZone.DTO.Responses;
using Microsoft.AspNetCore.Mvc;

namespace ChatZone.Repositories.Interfaces;

public interface IProfileRepository
{
    Task<Result<BlockedPersonResponse[]>> GetBlockedPersonsAsync(string username);
    Task<Result<IActionResult>> DeleteBlockedPersonAsync(string username, int idBlockedPerson);
    Task<Result<QuickMessageResponse[]>> GetQuickMessagesAsync(string username);
    Task<Result<QuickMessageResponse>> CreateQuickMessagesAsync(string username, string message);
}