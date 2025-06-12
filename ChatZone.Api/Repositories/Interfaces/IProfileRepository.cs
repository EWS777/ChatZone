using ChatZone.Core.Extensions;
using ChatZone.DTO.Responses;
using Microsoft.AspNetCore.Mvc;

namespace ChatZone.Repositories.Interfaces;

public interface IProfileRepository
{
    Task<Result<List<BlockedPersonResponse>>> GetBlockedPersonsAsync(int id);
    Task<Result<List<QuickMessageResponse>>> GetQuickMessagesAsync(int id);
    Task<Result<QuickMessageResponse>> AddQuickMessageAsync(int id, string message);
    Task<Result<IActionResult>> DeleteBlockedPersonAsync(int id, int idBlockedPerson);
}