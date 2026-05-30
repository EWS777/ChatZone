using ChatZone.Core.Extensions.Exceptions;
using ChatZone.Core.Models;
using ChatZone.Shared.Context;
using ChatZone.Shared.DTOs;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ChatZone.Features.QuickMessages.Create;

public class CreateQuickMessageHandler(
    ChatZoneDbContext dbContext)
    : IRequestHandler<CreateQuickMessageRequest, Result<CreateQuickMessageResponse>>
{
    public async Task<Result<CreateQuickMessageResponse>> Handle(CreateQuickMessageRequest request, CancellationToken cancellationToken)
    {
        var quickMessageAmount = await dbContext.QuickMessages.CountAsync(x => x.IdPerson == request.IdPerson, cancellationToken);
        if(quickMessageAmount >= 3) return Result<CreateQuickMessageResponse>.Failure(new IsExistsException("You can not create more than 3 quick messages!"));
        
        var quickMessage = new QuickMessage
        {
            Message = request.Message,
            IdPerson = request.IdPerson
        };
        
        await dbContext.QuickMessages.AddAsync(quickMessage, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);

        return Result<CreateQuickMessageResponse>.Ok(new CreateQuickMessageResponse
        {
            IdQuickMessage = quickMessage.IdQuickMessage,
            Message = quickMessage.Message
        });
    }
}