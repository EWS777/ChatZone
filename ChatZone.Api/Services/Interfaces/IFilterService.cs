using ChatZone.Core.Extensions;
using ChatZone.DTO.Requests;
using ChatZone.DTO.Responses;

namespace ChatZone.Services.Interfaces;

public interface IFilterService
{
    public Task<Result<PersonFilterResponse>> GetPersonFilterAsync(int id, CancellationToken cancellationToken);
    public Task<Result<PersonFilterResponse>> UpdatePersonFilterAsync(int id, PersonFilterRequest personFilterRequest, CancellationToken cancellationToken);
}