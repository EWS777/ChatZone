using ChatZone.Core.Extensions;
using ChatZone.DTO.Responses;
using ChatZone.Repositories.Interfaces;
using ChatZone.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ChatZone.Services;

public class ProfileService(IProfileRepository profileRepository,
                            IAuthRepository authRepository) : IProfileService
{
    public async Task<Result<ProfileResponse>> GetProfileAsync(int id, CancellationToken cancellationToken)
    {
        var person = await authRepository.GetPersonByIdAsync(id, cancellationToken);
        if (!person.IsSuccess) return Result<ProfileResponse>.Failure(person.Exception);

        return Result<ProfileResponse>.Ok(new ProfileResponse
        {
            Username = person.Value.Username,
            Email = person.Value.Email,
            IsFindByProfile = person.Value.IsFindByProfile
        });
    }

    public async Task<Result<List<BlockedPersonResponse>>> GetBlockedPersonsAsync(int id, CancellationToken cancellationToken)
    {
        var person = await authRepository.IsPersonExistsAsync(id, cancellationToken);
        if (!person.IsSuccess) return Result<List<BlockedPersonResponse>>.Failure(person.Exception);

        return await profileRepository.GetBlockedPersonsAsync(id, cancellationToken);
    }

    public async Task<Result<List<QuickMessageResponse>>> GetQuickMessagesAsync(int id, CancellationToken cancellationToken)
    {
        var person = await authRepository.IsPersonExistsAsync(id, cancellationToken);
        if (!person.IsSuccess) return Result<List<QuickMessageResponse>>.Failure(person.Exception);

        return await profileRepository.GetQuickMessagesAsync(id, cancellationToken);
    }

    public async Task<Result<QuickMessageResponse>> AddQuickMessageAsync(int id, string message, CancellationToken cancellationToken)
    {
        var person = await authRepository.IsPersonExistsAsync(id, cancellationToken);
        if (!person.IsSuccess) return Result<QuickMessageResponse>.Failure(person.Exception);

        return await profileRepository.AddQuickMessageAsync(id, message, cancellationToken);
    }

    public async Task<Result<IActionResult>> DeleteBlockedPersonAsync(int id, int idBlockedPerson, CancellationToken cancellationToken)
    {
        var person = await authRepository.IsPersonExistsAsync(id, cancellationToken);
        if (!person.IsSuccess) return Result<IActionResult>.Failure(person.Exception);

        return await profileRepository.DeleteBlockedPersonAsync(id, idBlockedPerson, cancellationToken);
    }
}