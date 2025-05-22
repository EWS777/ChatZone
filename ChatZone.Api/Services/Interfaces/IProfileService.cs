using ChatZone.Core.Extensions;
using ChatZone.DTO.Responses;
using Microsoft.AspNetCore.Mvc;

namespace ChatZone.Services.Interfaces;

public interface IProfileService
{
    public Task<Result<ProfileResponse>> GetProfileAsync(string username);
    public Task<Result<BlockedPersonResponse[]>> GetBlockedPersonsAsync(string username);
    public Task<Result<IActionResult>> DeleteBlockedPersonAsync(string username, int idBlockedPerson);
    public Task<Result<QuickMessageResponse[]>> GetQuickMessagesAsync(string username);
    public Task<Result<QuickMessageResponse>> CreateQuickMessagesAsync(string username, string message);
}