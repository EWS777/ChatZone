using ChatZone.Core.Extensions;
using ChatZone.DTO.Requests;
using ChatZone.DTO.Responses;

namespace ChatZone.Services.Interfaces;

public interface IFilterService
{
    public Task<Result<PersonFilterResponse>> GetPersonFilterAsync(string username);
    public Task<Result<PersonFilterResponse>> UpdatePersonFilterAsync(string username, PersonFilterRequest personFilterRequest);
}