using ChatZone.Core.Extensions;
using ChatZone.Core.Models;
using ChatZone.DTO.Requests;
using ChatZone.DTO.Responses;
using Microsoft.AspNetCore.Mvc;

namespace ChatZone.Repositories.Interfaces;

public interface IProfileRepository
{
    Task<Result<UpdateProfileResponse>> UpdateProfileAsync(string username, ProfileRequest profileRequest);
    Task<Result<BlockedPerson[]>> GetBlockedPersonsAsync(string username);
    Task<Result<IActionResult>> DeleteBlockedPersonAsync(string username, int idBlockedPerson);
    Task<Result<QuickMessage[]>> GetQuickMessagesAsync(string username);
    Task<Result<QuickMessage>> CreateQuickMessagesAsync(string username, string message);
}