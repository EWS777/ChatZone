using ChatZone.Context;
using ChatZone.Core.Extensions;
using ChatZone.Core.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ChatZone.Features.Search.Find;

public class FindPersonHandler(ChatZoneDbContext dbContext)
    : IRequestHandler<FindPersonRequest, Result<IActionResult>>
{
    public async Task<Result<IActionResult>> Handle(FindPersonRequest request, CancellationToken cancellationToken)
    { 
        await dbContext.MatchQueues.AddAsync(new MatchQueue
        {
            IdPerson = request.IdPerson,
            ThemeList = request.ThemeList,
            Country = request.Country,
            City = request.City,
            Age = request.Age,
            Gender = request.Gender,
            Lang = request.Lang
        }, cancellationToken);

        await dbContext.SaveChangesAsync(cancellationToken);
        return Result<IActionResult>.Ok(new ObjectResult(new {message = "You've added to the list!"}));
    }
}