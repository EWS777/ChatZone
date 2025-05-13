using ChatZone.Core.Extensions;
using ChatZone.DTO.Requests;
using ChatZone.DTO.Responses;
using ChatZone.Repositories.Interfaces;
using ChatZone.Services.Interfaces;

namespace ChatZone.Services;

public class FilterService(IFilterRepository filterRepository,
                            IAuthRepository authRepository) : IFilterService
{
    public async Task<Result<PersonFilterResponse>> GetPersonFilterAsync(string username)
    {
        var user = await authRepository.GetPersonByUsernameAsync(username);
        if (!user.IsSuccess) return Result<PersonFilterResponse>.Failure(user.Exception);

        var userResult = await filterRepository.GetPersonFilterAsync(user.Value.Username);
        if (!userResult.IsSuccess) return Result<PersonFilterResponse>.Failure(userResult.Exception);

        return userResult;
    }

    public async Task<Result<PersonFilterResponse>> UpdatePersonFilterAsync(string username, PersonFilterRequest personFilterRequest)
    {
        var person = await authRepository.GetPersonByUsernameAsync(username);
        if (!person.IsSuccess) return Result<PersonFilterResponse>.Failure(person.Exception);

        return await filterRepository.UpdatePersonFilterAsync(username, personFilterRequest);
    }
}