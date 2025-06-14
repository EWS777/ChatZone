using ChatZone.Context;
using ChatZone.Core.Extensions;
using ChatZone.Core.Extensions.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ChatZone.Features.Filters.Update;

public class UpdateFilterHandler(ChatZoneDbContext dbContext) : IRequestHandler<UpdateFilterRequest, Result<UpdateFilterResponse>>
{
    public async Task<Result<UpdateFilterResponse>> Handle(UpdateFilterRequest request, CancellationToken cancellationToken)
    {
        var person = await dbContext.Persons.SingleOrDefaultAsync(x => x.IdPerson == request.Id, cancellationToken);
        if (person is null) return Result<UpdateFilterResponse>.Failure(new NotFoundException("User is not found!"));
        
        person.ThemeList = request.ThemeList;
        person.Country = request.Country;
        person.City = request.City;
        person.Age = request.Age;
        person.Gender = request.Gender;
        person.NativeLang = request.NativeLang;
        person.LearnLang = request.LearnLang;

        dbContext.Persons.Update(person);
        await dbContext.SaveChangesAsync(cancellationToken);
        
        return Result<UpdateFilterResponse>.Ok(new UpdateFilterResponse
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