using ChatZone.Core.Extensions.Exceptions;
using ChatZone.Shared.Context;
using ChatZone.Shared.DTOs;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ChatZone.Features.Filters.Update;

public class UpdateFilterHandler(ChatZoneDbContext dbContext) : IRequestHandler<UpdateFilterRequest, Result<bool>>
{
    public async Task<Result<bool>> Handle(UpdateFilterRequest request, CancellationToken cancellationToken)
    {
        var person = await dbContext.Persons.SingleOrDefaultAsync(x => x.IdPerson == request.IdPerson, cancellationToken);
        if (person is null) return Result<bool>.Failure(new NotFoundException("User is not found!"));
        
        person.Theme = request.Theme;
        person.Country = request.Country;
        person.City = request.City;
        person.Age = request.Age;
        person.YourGender = request.YourGender;
        person.PartnerGender = request.PartnerGender;
        person.Language = request.Language;
        person.IsFindRandomPerson = request.IsFindRandomPerson;

        dbContext.Persons.Update(person);
        await dbContext.SaveChangesAsync(cancellationToken);
        
        return Result<bool>.Ok(true);
    }
}