using ChatZone.Shared.Context;
using ChatZone.Shared.DTOs;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ChatZone.Features.QuickMessages.GetList;

public class GetQuickMessageListHandler(
    ChatZoneDbContext dbContext) : IRequestHandler<GetQuickMessageListRequest, Result<List<GetQuickMessageListResponse>>>
{
    public async Task<Result<List<GetQuickMessageListResponse>>> Handle(GetQuickMessageListRequest request, CancellationToken cancellationToken)
    {
        var quickMessageList = await dbContext.QuickMessages
            .AsNoTracking()
            .Where(x => x.IdPerson == request.IdPerson)
            .Select(x => new GetQuickMessageListResponse
            {
                IdQuickMessage = x.IdQuickMessage,
                Message = x.Message
            })
            .ToListAsync(cancellationToken);

        return Result<List<GetQuickMessageListResponse>>.Ok(quickMessageList);
    }
}