using ChatZone.Core.Extensions.Exceptions;
using ChatZone.Features.Filters.Update;
using ChatZone.Features.Search.Cancel;
using ChatZone.Matchmaking;
using ChatZone.Shared.Context;
using ChatZone.Shared.DTOs;
using ChatZone.Shared.Hubs;
using MediatR;
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
        var prepareResult = await PrepareRequestData(request, cancellationToken);
        if (!prepareResult.IsSuccess) return Result<bool>.Failure(prepareResult.Exception);
        
        try
        {
            var maxAttempts = 20;
            for (var attempt = 0; attempt < maxAttempts; attempt++)
            {
                cancellationToken.ThrowIfCancellationRequested();

                if (attempt > 0)
                {
                    var statusResult = await CheckSearchStatus(request.IdPerson, cancellationToken);
                    if (statusResult != null) return statusResult;
                }
                
                var matchResult = await matchmakingService.FindMatch(request, cancellationToken);
                
                if (!matchResult.IsSuccess) return Result<bool>.Failure(matchResult.Exception);

                if (matchResult.Value.IsFound)
                {
                    var person1 = matchResult.Value.Person1!;
                    var person2 = matchResult.Value.Person2!;
                    var idGroup = matchResult.Value.IdGroup!.Value;
                    
                    await hubContext.Groups.AddToGroupAsync(person1.ConnectionId, idGroup.ToString(), cancellationToken);
                    await hubContext.Groups.AddToGroupAsync(person2.ConnectionId, idGroup.ToString(), cancellationToken);

                    await hubContext.Clients.Group(idGroup.ToString()).SendAsync("ChatCreated", cancellationToken);
                    return Result<bool>.Ok(true);
                }
                
                request.IsSearchAgain = true;
                dbContext.ChangeTracker.Clear();
                
                var delayAttempt = await WaitForNextAttempt(request.IdPerson, cancellationToken);
                if(!delayAttempt) return Result<bool>.Failure(new OperationCanceledException("Finding has cancelled by Person!"));
            }
            
            await mediator.Send(new CancelPersonRequest { IdPerson = request.IdPerson }, CancellationToken.None);
            return Result<bool>.Ok(false);
        }
        catch
        {
            await mediator.Send(new CancelPersonRequest { IdPerson = request.IdPerson }, CancellationToken.None);
            throw;
        }
    }
    
    private async Task<Result<bool>> PrepareRequestData(FindPersonRequest request, CancellationToken cancellationToken)
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
            
            if (!savedPersonFilter.IsSuccess) return Result<bool>.Failure(savedPersonFilter.Exception);
        }

        else
        {
            var result = await dbContext.Persons
                .AsNoTracking()
                .Where(x => x.IdPerson == request.IdPerson)
                .Select(x => new
                {
                    x.Theme, x.Country, x.City, x.Age, x.YourGender, x.PartnerGender, x.Language, x.IsFindRandomPerson
                })
                .SingleOrDefaultAsync(cancellationToken);
            
            if (result == null) return Result<bool>.Failure(new NotFoundException("User is not found!"));

            request.Theme = result.Theme;
            request.Country = result.Country;
            request.City = result.City;
            request.Age = result.Age;
            request.YourGender = result.YourGender;
            request.PartnerGender = result.PartnerGender;
            request.Language = result.Language;
            request.IsFindRandomPerson = result.IsFindRandomPerson;
        }
        
        return Result<bool>.Ok(true);
    }

    private async Task<bool> WaitForNextAttempt(int idPerson, CancellationToken cancellationToken)
    {
        try
        {
            await Task.Delay(3000, cancellationToken);
            return true;
        }
        catch (OperationCanceledException)
        {
            await mediator.Send(new CancelPersonRequest { IdPerson = idPerson }, CancellationToken.None);
            return false;
        }
    }

    private async Task<Result<bool>?> CheckSearchStatus(int idPerson, CancellationToken cancellationToken)
    {
        var isStillSearching = await dbContext.MatchQueues
            .AsNoTracking()
            .Where(x => x.IdPerson == idPerson)
            .AnyAsync(cancellationToken);

        if (isStillSearching) return null;
        
        var hasActiveChat = await dbContext.SingleChats
            .Where(x => (x.IdSecondPerson == idPerson || x.IdFirstPerson == idPerson) && x.FinishedAt == null)
            .AnyAsync(cancellationToken);
        
        return hasActiveChat ? Result<bool>.Ok(true) : Result<bool>.Failure(new OperationCanceledException("Finding has cancelled by Person!"));
    }
}