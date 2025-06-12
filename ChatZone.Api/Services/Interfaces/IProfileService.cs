using ChatZone.Core.Extensions;
using ChatZone.DTO.Responses;
using Microsoft.AspNetCore.Mvc;

namespace ChatZone.Services.Interfaces;

public interface IProfileService
{
    public Task<Result<ProfileResponse>> GetProfileAsync(int id);
    public Task<Result<List<BlockedPersonResponse>>> GetBlockedPersonsAsync(int id);
    public Task<Result<List<QuickMessageResponse>>> GetQuickMessagesAsync(int id);
    public Task<Result<QuickMessageResponse>> AddQuickMessageAsync(int id, string message);
    public Task<Result<IActionResult>> DeleteBlockedPersonAsync(int id, int idBlockedPerson);
}