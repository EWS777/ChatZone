using ChatZone.Core.Extensions;
using ChatZone.DTO.Responses;
using Microsoft.AspNetCore.Mvc;

namespace ChatZone.Services.Interfaces;

public interface IProfileService
{
    public Task<Result<ProfileResponse>> GetProfileAsync(int id, CancellationToken cancellationToken);
    public Task<Result<List<BlockedPersonResponse>>> GetBlockedPersonsAsync(int id, CancellationToken cancellationToken);
    public Task<Result<List<QuickMessageResponse>>> GetQuickMessagesAsync(int id, CancellationToken cancellationToken);
    public Task<Result<QuickMessageResponse>> AddQuickMessageAsync(int id, string message, CancellationToken cancellationToken);
    public Task<Result<IActionResult>> DeleteBlockedPersonAsync(int id, int idBlockedPerson, CancellationToken cancellationToken);
}