using ChatZone.Core.Extensions;
using ChatZone.DTO.Responses;

namespace ChatZone.Services.Interfaces;

public interface IFilterService
{
    public Task<Result<PersonFilterResponse>> GetPersonFilterAsync(string usernameToken, string username);
}