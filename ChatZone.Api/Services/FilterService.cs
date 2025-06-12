using ChatZone.Core.Extensions;
using ChatZone.DTO.Requests;
using ChatZone.DTO.Responses;
using ChatZone.Repositories.Interfaces;
using ChatZone.Services.Interfaces;

namespace ChatZone.Services;

public class FilterService(IFilterRepository filterRepository,
                            IAuthRepository authRepository) : IFilterService
{
    public async Task<Result<PersonFilterResponse>> GetPersonFilterAsync(int id)
    {
        var user = await authRepository.IsPersonExistsAsync(id);
        if (!user.IsSuccess) return Result<PersonFilterResponse>.Failure(user.Exception);
        
        return await filterRepository.GetPersonFilterAsync(id);
    }

    public async Task<Result<PersonFilterResponse>> UpdatePersonFilterAsync(int id, PersonFilterRequest personFilterRequest)
    {
        var person = await authRepository.IsPersonExistsAsync(id);
        if (!person.IsSuccess) return Result<PersonFilterResponse>.Failure(person.Exception);

        return await filterRepository.UpdatePersonFilterAsync(id, personFilterRequest);
    }
}