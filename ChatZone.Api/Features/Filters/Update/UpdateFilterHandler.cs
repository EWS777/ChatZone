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
        
        person.Theme = request.Theme;
        person.Country = request.Country;
        person.City = request.City;
        person.Age = request.Age;
        person.YourGender = request.YourGender;
        person.PartnerGender = request.PartnerGender;
        person.Language = request.Language;

        dbContext.Persons.Update(person);
        await dbContext.SaveChangesAsync(cancellationToken);
        
        return Result<UpdateFilterResponse>.Ok(new UpdateFilterResponse
        {
            Theme = person.Theme,
            Age = person.Age,
            City = person.City,
            Country = person.Country,
            YourGender = person.YourGender,
            PartnerGender = person.PartnerGender,
            Language = person.Language
        });
    }
}