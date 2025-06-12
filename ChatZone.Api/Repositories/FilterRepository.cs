using ChatZone.Context;
using ChatZone.Core.Extensions;
using ChatZone.DTO.Requests;
using ChatZone.DTO.Responses;
using ChatZone.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ChatZone.Repositories;

public class FilterRepository(ChatZoneDbContext dbContext) : IFilterRepository
{
    public async Task<Result<PersonFilterResponse>> GetPersonFilterAsync(int id, CancellationToken cancellationToken)
    {
        var personFilter = await dbContext.Persons.SingleOrDefaultAsync(x => x.IdPerson == id, cancellationToken);
        return Result<PersonFilterResponse>.Ok(new PersonFilterResponse
        {
            ThemeList = personFilter!.ThemeList,
            Country = personFilter.Country,
            City = personFilter.City,
            Age = personFilter.Age,
            Gender = personFilter.Gender,
            NativeLang = personFilter.NativeLang,
            LearnLang = personFilter.LearnLang
        });
    }

    public async Task<Result<PersonFilterResponse>> UpdatePersonFilterAsync(int id, PersonFilterRequest personFilterRequest, CancellationToken cancellationToken)
    {
        var person = await dbContext.Persons.SingleOrDefaultAsync(x => x.IdPerson == id, cancellationToken);

        person!.ThemeList = personFilterRequest.ThemeList;
        person.Country = personFilterRequest.Country;
        person.City = personFilterRequest.City;
        person.Age = personFilterRequest.Age;
        person.Gender = personFilterRequest.Gender;
        person.NativeLang = personFilterRequest.NativeLang;
        person.LearnLang = personFilterRequest.LearnLang;

        dbContext.Persons.Update(person);
        await dbContext.SaveChangesAsync(cancellationToken);
        
        return Result<PersonFilterResponse>.Ok(new PersonFilterResponse
        {
            ThemeList = person.ThemeList,
            Age = person.Age,
            City = person.City,
            Country = person.Country,
            Gender = person.Gender,
            LearnLang = person.LearnLang,
            NativeLang = person.NativeLang
        });
    }
}