using ChatZone.Context;
using ChatZone.Core.Extensions;
using ChatZone.Core.Extensions.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ChatZone.Features.QuickMessages.Update;

public class UpdateQuickMessageHandler
    (ChatZoneDbContext dbContext) : IRequestHandler<UpdateQuickMessageRequest, Result<UpdateQuickMessageResponse>>
{
    public async Task<Result<UpdateQuickMessageResponse>> Handle(UpdateQuickMessageRequest request, CancellationToken cancellationToken)
    {
        var person = await dbContext.Persons.AnyAsync(x => x.IdPerson == request.IdPerson, cancellationToken);
        if(!person) return Result<UpdateQuickMessageResponse>.Failure(new NotFoundException("User is not found!"));

        var quickMessage = await dbContext.QuickMessages.SingleOrDefaultAsync(x => x.IdQuickMessage == request.IdQuickMessage,
                cancellationToken);
        if(quickMessage is null) return Result<UpdateQuickMessageResponse>.Failure(new NotFoundException("Quick message is not found!"));

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