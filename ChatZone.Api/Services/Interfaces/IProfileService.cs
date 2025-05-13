using ChatZone.Core.Extensions;
using ChatZone.Core.Models;
using ChatZone.DTO.Requests;
using ChatZone.DTO.Responses;
using Microsoft.AspNetCore.Mvc;

namespace ChatZone.Services.Interfaces;

public interface IProfileService
{
    public Task<Result<ProfileResponse>> GetProfileAsync(string username);
    public Task<Result<UpdateProfileResponse>> UpdateProfileAsync(string username, ProfileRequest profileRequest);
    public Task<Result<BlockedPerson[]>> GetBlockedPersonsAsync(string username);
    public Task<Result<IActionResult>> DeleteBlockedPersonAsync(string username, int idBlockedPerson);
    public Task<Result<QuickMessage[]>> GetQuickMessagesAsync(string username);
    public Task<Result<QuickMessage>> CreateQuickMessagesAsync(string username, string message);
}