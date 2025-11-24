using ChatZone.Core.Models;
using ChatZone.Shared.Context;
using ChatZone.Shared.DTOs;
using MediatR;

namespace ChatZone.Features.QuickMessages.Create;

public class CreateQuickMessageHandler(
    ChatZoneDbContext dbContext)
    : IRequestHandler<CreateQuickMessageRequest, Result<CreateQuickMessageResponse>>
{
    public async Task<Result<CreateQuickMessageResponse>> Handle(CreateQuickMessageRequest request, CancellationToken cancellationToken)
    {
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