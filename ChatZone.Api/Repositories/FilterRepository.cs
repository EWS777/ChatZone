using ChatZone.Context;
using ChatZone.Core.Extensions;
using ChatZone.Core.Extensions.Exceptions;
using ChatZone.Core.Models;
using ChatZone.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ChatZone.Repositories;

public class FilterRepository(ChatZoneDbContext dbContext,
                                IAuthRepository authRepository) : IFilterRepository
{
    public async Task<Result<PersonData>> GetPersonFilterAsync(string username)
    {
        var user = await authRepository.GetPersonByUsernameAsync(username);

        var personFilter = await dbContext.PersonDatas.SingleOrDefaultAsync(x => x.PersonDataId == user.Value.PersonId);
        if (personFilter is null)
            return Result<PersonData>.Failure(new NotAnyDataException("The filters data aren't exist"));
        
        return Result<PersonData>.Ok(personFilter);
    }
}