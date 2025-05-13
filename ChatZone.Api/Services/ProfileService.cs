using ChatZone.Core.Extensions;
using ChatZone.Core.Extensions.Exceptions;
using ChatZone.Core.Models;
using ChatZone.DTO.Requests;
using ChatZone.DTO.Responses;
using ChatZone.Repositories.Interfaces;
using ChatZone.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ChatZone.Services;

public class ProfileService(IProfileRepository profileRepository,
                            IAuthRepository authRepository) : IProfileService
{
    public async Task<Result<ProfileResponse>> GetProfileAsync(string username)
    {
        var person = await authRepository.GetPersonByUsernameAsync(username);
        if (!person.IsSuccess) return Result<ProfileResponse>.Failure(person.Exception);

        return Result<ProfileResponse>.Ok(new ProfileResponse
        {
            Username = person.Value.Username,
            Email = person.Value.Email
        });
    }

    public async Task<Result<UpdateProfileResponse>> UpdateProfileAsync(string username, ProfileRequest profileRequest)
    {
        var person = await authRepository.GetPersonByUsernameAsync(username);
        if (!person.IsSuccess) return Result<UpdateProfileResponse>.Failure(person.Exception);


        var isUsernameIsNotUsed = await authRepository.GetPersonByUsernameAsync(profileRequest.Username);
        if (isUsernameIsNotUsed.IsSuccess) return Result<UpdateProfileResponse>.Failure(new IsExistsException("This username is exists!"));

        var updatePerson = await profileRepository.UpdateProfileAsync(username, profileRequest);

        return Result<UpdateProfileResponse>.Ok(updatePerson.Value);
    }

    public async Task<Result<BlockedPerson[]>> GetBlockedPersonsAsync(string username)
    {
        var person = await authRepository.GetPersonByUsernameAsync(username);
        if (!person.IsSuccess) return Result<BlockedPerson[]>.Failure(person.Exception);

        return await profileRepository.GetBlockedPersonsAsync(username);
    }

    public async Task<Result<IActionResult>> DeleteBlockedPersonAsync(string username, int idBlockedPerson)
    {
        var person = await authRepository.GetPersonByUsernameAsync(username);
        if (!person.IsSuccess) return Result<IActionResult>.Failure(person.Exception);

        return await profileRepository.DeleteBlockedPersonAsync(username, idBlockedPerson);
    }

    public async Task<Result<QuickMessage[]>> GetQuickMessagesAsync(string username)
    {
        var person = await authRepository.GetPersonByUsernameAsync(username);
        if (!person.IsSuccess) return Result<QuickMessage[]>.Failure(person.Exception);

        return await profileRepository.GetQuickMessagesAsync(username);
    }

    public async Task<Result<QuickMessage>> CreateQuickMessagesAsync(string username, string message)
    {
        var person = await authRepository.GetPersonByUsernameAsync(username);
        if (!person.IsSuccess) return Result<QuickMessage>.Failure(person.Exception);

        return await profileRepository.CreateQuickMessagesAsync(username, message);
    }
}