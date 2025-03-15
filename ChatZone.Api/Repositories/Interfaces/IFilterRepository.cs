using ChatZone.Core.Extensions;
using ChatZone.Core.Models;

namespace ChatZone.Repositories.Interfaces;

public interface IFilterRepository
{
    public Task<Result<PersonData>> GetPersonFilterAsync(string username);
}