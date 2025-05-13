using ChatZone.Context;
using ChatZone.Core.Extensions;
using ChatZone.Core.Extensions.Exceptions;
using ChatZone.DTO.Requests;
using ChatZone.DTO.Responses;
using ChatZone.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ChatZone.Repositories;

public class FilterRepository(ChatZoneDbContext dbContext,
                                IAuthRepository authRepository) : IFilterRepository
{
    public async Task<Result<PersonFilterResponse>> GetPersonFilterAsync(string username)
    {
        var user = await authRepository.GetPersonByUsernameAsync(username);
        var personFilter = await dbContext.Persons.SingleOrDefaultAsync(x => x.IdPerson == user.Value.IdPerson);
        if (personFilter is null)
            return Result<PersonFilterResponse>.Failure(new NotAnyDataException("The filters data aren't exist"));
        
        return Result<PersonFilterResponse>.Ok(new PersonFilterResponse
        {
            Country = user.Value.Country,
            City = user.Value.City,
            Age = user.Value.Age,
            Gender = user.Value.Gender,
            NativeLang = user.Value.NativeLang,
            LearnLang = user.Value.LearnLang
        });
    }

    public async Task<Result<PersonFilterResponse>> UpdatePersonFilterAsync(string username, PersonFilterRequest personFilterRequest)
    {
        var person = await authRepository.GetPersonByUsernameAsync(username);

        person.Value.Country = personFilterRequest.Country;
        person.Value.City = personFilterRequest.City;
        person.Value.Age = personFilterRequest.Age;
        person.Value.Gender = personFilterRequest.Gender;
        person.Value.NativeLang = personFilterRequest.NativeLang;
        person.Value.LearnLang = personFilterRequest.LearnLang;

        dbContext.Persons.Update(person.Value);
        await dbContext.SaveChangesAsync();
        return Result<PersonFilterResponse>.Ok(new PersonFilterResponse
        {
            Age = person.Value.Age,
            City = person.Value.City,
            Country = person.Value.Country,
            Gender = person.Value.Gender,
            LearnLang = person.Value.LearnLang,
            NativeLang = person.Value.NativeLang
        });
    }
}