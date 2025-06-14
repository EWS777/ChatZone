using ChatZone.Context;
using ChatZone.Core.Extensions;
using ChatZone.Core.Extensions.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ChatZone.Features.Filters.Get;

public class GetFilterHandler(ChatZoneDbContext dbContext) : IRequestHandler<GetFilterRequest, Result<GetFilterResponse>>
{
    public async Task<Result<GetFilterResponse>> Handle(GetFilterRequest request, CancellationToken cancellationToken)
    {
        var person = await dbContext.Persons.SingleOrDefaultAsync(x=>x.IdPerson==request.Id, cancellationToken);
        if(person is null) return Result<GetFilterResponse>.Failure(new NotFoundException("User is not found!"));

        return Result<GetFilterResponse>.Ok(new GetFilterResponse
        {
            ThemeList = person!.ThemeList,
            Country = person.Country,
            City = person.City,
            Age = person.Age,
            Gender = person.Gender,
            NativeLang = person.NativeLang,
            LearnLang = person.LearnLang
        });
    }
}