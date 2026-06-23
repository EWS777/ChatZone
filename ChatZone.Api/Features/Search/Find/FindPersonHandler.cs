using ChatZone.Core.Extensions.Exceptions;
using ChatZone.Features.Filters.Update;
using ChatZone.Matchmaking;
using ChatZone.Shared.Context;
using ChatZone.Shared.DTOs;
using ChatZone.Shared.Hubs;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace ChatZone.Features.Search.Find;

public class FindPersonHandler(
    ChatZoneDbContext dbContext,
    IMatchmakingService matchmakingService,
    IHubContext<ChatZoneHub> hubContext,
    IMediator mediator)
    : IRequestHandler<FindPersonRequest, Result<bool>>
{
    public async Task<Result<bool>> Handle(FindPersonRequest request, CancellationToken cancellationToken)
    {
        if (!request.IsSearchAgain)
        {
            var savedPersonFilter = await mediator.Send(new UpdateFilterRequest
            {
                IdPerson = request.IdPerson,
                Theme = request.Theme,
                Country = request.Country,
                City = request.City,
                Age = request.Age,
                YourGender = request.YourGender,
                PartnerGender = request.PartnerGender,
                Language = request.Language,
                IsFindRandomPerson = request.IsFindRandomPerson
            }, cancellationToken);
            
            if(!savedPersonFilter.IsSuccess) return Result<bool>.Failure(savedPersonFilter.Exception);
        }
        if (request.IsSearchAgain)
        {
            var result = await dbContext.Persons
                .AsNoTracking()
                .Where(x => x.IdPerson == request.IdPerson)
                .Select(x => new { x.Theme, x.Country, x.City, x.Age, x.YourGender, x.PartnerGender, x.Language, x.IsFindRandomPerson })
                .SingleOrDefaultAsync(cancellationToken);
            
            if(result == null) return Result<bool>.Failure(new NotFoundException("User is not found!"));

            request.Theme = result.Theme;
            request.Country = result.Country;
            request.City = result.City;
            request.Age = result.Age;
            request.YourGender = result.YourGender;
            request.PartnerGender = result.PartnerGender;
            request.Language = result.Language;
            request.IsFindRandomPerson = result.IsFindRandomPerson;
        }
        
        int maxAttempts = 20;
        for (int attempt = 0; attempt < maxAttempts; attempt++)
        {
            cancellationToken.ThrowIfCancellationRequested();
            
            var matchResult = await matchmakingService.FindMatch(request, cancellationToken);
        
            if(!matchResult.IsSuccess) return Result<bool>.Failure(matchResult.Exception);

            if (matchResult.Value.IsFound)
            {
                var person1 = matchResult.Value.Person1!;
                var person2 = matchResult.Value.Person2!;
                var idGroup = matchResult.Value.IdGroup!.Value;
        
                await hubContext.Groups.AddToGroupAsync(person1.ConnectionId, idGroup.ToString(), cancellationToken);
                await hubContext.Groups.AddToGroupAsync(person2.ConnectionId, idGroup.ToString(), cancellationToken);

                await hubContext.Clients.Group(idGroup.ToString()).SendAsync("ChatCreated", cancellationToken: cancellationToken);
                return Result<bool>.Ok(true);
            }
            
            request.IsSearchAgain = true;
        
            dbContext.ChangeTracker.Clear();

            await Task.Delay(3000, cancellationToken);
        }
        
        return Result<bool>.Ok(false);
    }
}