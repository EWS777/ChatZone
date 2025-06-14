using ChatZone.Context;
using ChatZone.Core.Extensions;
using ChatZone.Core.Extensions.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ChatZone.Features.QuickMessages.GetList;

public class GetQuickMessageListHandler(
    ChatZoneDbContext dbContext) : IRequestHandler<GetQuickMessageListRequest, Result<List<GetQuickMessageListResponse>>>
{
    public async Task<Result<List<GetQuickMessageListResponse>>> Handle(GetQuickMessageListRequest request, CancellationToken cancellationToken)
    {
        var isPersonExists = await dbContext.Persons.AnyAsync(x => x.IdPerson == request.Id, cancellationToken);
        if (!isPersonExists) return Result<List<GetQuickMessageListResponse>>.Failure(new NotFoundException("User is not found!"));
        
        var quickMessageList = await dbContext.QuickMessages
            .Where(x => x.IdPerson == request.Id)
            .Select(x => new GetQuickMessageListResponse
            {
                IdQuickMessage = x.IdQuickMessage,
                Message = x.Message
            })
            .ToListAsync(cancellationToken);

        return Result<List<GetQuickMessageListResponse>>.Ok(quickMessageList);
    }
}