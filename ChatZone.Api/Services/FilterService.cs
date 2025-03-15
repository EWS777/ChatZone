using ChatZone.Core.Extensions;
using ChatZone.Core.Extensions.Exceptions;
using ChatZone.DTO.Responses;
using ChatZone.Repositories.Interfaces;
using ChatZone.Services.Interfaces;

namespace ChatZone.Services;

public class FilterService(IFilterRepository filterRepository,
                            IAuthRepository authRepository) : IFilterService
{
    public async Task<Result<PersonFilterResponse>> GetPersonFilterAsync(string usernameToken, string username)
    {
        if (usernameToken != username) return Result<PersonFilterResponse>.Failure(new ForbiddenAccessException("You are not an owner"));

        var user = await authRepository.GetPersonByUsernameAsync(username);
        if (!user.IsSuccess) return Result<PersonFilterResponse>.Failure(user.Exception);

        var userResult = await filterRepository.GetPersonFilterAsync(user.Value.Username);
        if (!userResult.IsSuccess) return Result<PersonFilterResponse>.Failure(userResult.Exception);
        
        return Result<PersonFilterResponse>.Ok(new PersonFilterResponse
        {
            Country = userResult.Value.Country,
            City = userResult.Value.City,
            Age = userResult.Value.Age,
            Gender = userResult.Value.Gender,
            NativeLang = userResult.Value.NativeLang,
            LearnLang = userResult.Value.LearnLang
        });
    }
}