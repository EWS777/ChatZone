using ChatZone.Core.Extensions;
using ChatZone.DTO.Responses;
using ChatZone.Repositories.Interfaces;
using ChatZone.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ChatZone.Services;

public class ProfileService(IProfileRepository profileRepository,
                            IAuthRepository authRepository) : IProfileService
{
    public async Task<Result<ProfileResponse>> GetProfileAsync(int id)
    {
        var person = await authRepository.GetPersonByIdAsync(id);
        if (!person.IsSuccess) return Result<ProfileResponse>.Failure(person.Exception);

        return Result<ProfileResponse>.Ok(new ProfileResponse
        {
            Username = person.Value.Username,
            Email = person.Value.Email,
            IsFindByProfile = person.Value.IsFindByProfile
        });
    }

    public async Task<Result<List<BlockedPersonResponse>>> GetBlockedPersonsAsync(int id)
    {
        var person = await authRepository.IsPersonExistsAsync(id);
        if (!person.IsSuccess) return Result<List<BlockedPersonResponse>>.Failure(person.Exception);

        return await profileRepository.GetBlockedPersonsAsync(id);
    }

    public async Task<Result<List<QuickMessageResponse>>> GetQuickMessagesAsync(int id)
    {
        var person = await authRepository.IsPersonExistsAsync(id);
        if (!person.IsSuccess) return Result<List<QuickMessageResponse>>.Failure(person.Exception);

        return await profileRepository.GetQuickMessagesAsync(id);
    }

    public async Task<Result<QuickMessageResponse>> AddQuickMessageAsync(int id, string message)
    {
        var person = await authRepository.IsPersonExistsAsync(id);
        if (!person.IsSuccess) return Result<QuickMessageResponse>.Failure(person.Exception);

        return await profileRepository.AddQuickMessageAsync(id, message);
    }

    public async Task<Result<IActionResult>> DeleteBlockedPersonAsync(int id, int idBlockedPerson)
    {
        var person = await authRepository.IsPersonExistsAsync(id);
        if (!person.IsSuccess) return Result<IActionResult>.Failure(person.Exception);

        return await profileRepository.DeleteBlockedPersonAsync(id, idBlockedPerson);
    }
}