using ChatZone.Context;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ChatZone.Features.Messages.GetStatus;

public class GetStatusHandler(
    ChatZoneDbContext dbContext) : IRequestHandler<GetStatusRequest, bool>
{
    public async Task<bool> Handle(GetStatusRequest request, CancellationToken cancellationToken)
    {
        var result = await (request.IsSingleChat 
            ? dbContext.SingleMessages.AnyAsync(x => x.IdChat == request.IdChat && x.IdSender == request.IdPerson, cancellationToken) 
            : dbContext.GroupMessages.AnyAsync(x => x.IdChat == request.IdChat && x.IdSender == request.IdPerson, cancellationToken));
        
        return result;
    }
}