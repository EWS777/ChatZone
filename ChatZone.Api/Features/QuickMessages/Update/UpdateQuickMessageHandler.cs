using ChatZone.Core.Extensions.Exceptions;
using ChatZone.Shared.Context;
using ChatZone.Shared.DTOs;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ChatZone.Features.QuickMessages.Update;

public class UpdateQuickMessageHandler
    (ChatZoneDbContext dbContext) : IRequestHandler<UpdateQuickMessageRequest, Result<UpdateQuickMessageResponse>>
{
    public async Task<Result<UpdateQuickMessageResponse>> Handle(UpdateQuickMessageRequest request, CancellationToken cancellationToken)
    {
        var quickMessage = await dbContext.QuickMessages.SingleOrDefaultAsync(x => x.IdPerson == request.IdPerson && x.IdQuickMessage == request.IdQuickMessage, cancellationToken);
        if(quickMessage is null) return Result<UpdateQuickMessageResponse>.Failure(new ForbiddenAccessException("Quick message is not found or you are not an owner of this quick message!"));

        quickMessage.Message = request.Message;
        dbContext.QuickMessages.Update(quickMessage);

        await dbContext.SaveChangesAsync(cancellationToken);
        return Result<UpdateQuickMessageResponse>.Ok(new UpdateQuickMessageResponse
        {
            IdQuickMessage = quickMessage.IdQuickMessage,
            Message = quickMessage.Message
        });
    }
}