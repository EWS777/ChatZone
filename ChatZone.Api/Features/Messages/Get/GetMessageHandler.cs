using ChatZone.Context;
using ChatZone.Core.Extensions;
using ChatZone.Core.Extensions.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ChatZone.Features.Messages.Get;

public class GetMessageHandler(
    ChatZoneDbContext dbContext) : IRequestHandler<GetMessageRequest, Result<GetMessageResponse>>
{
    public async Task<Result<GetMessageResponse>> Handle(GetMessageRequest request, CancellationToken cancellationToken)
    {
        var isPersonExists = request.IsSingleChat
            ? await dbContext.SingleChats.AnyAsync(x => x.IdSingleChat == request.IdChat && 
                                                    (x.IdFirstPerson == request.IdPerson || x.IdSecondPerson == request.IdPerson), cancellationToken)
            : await dbContext.GroupMembers.AnyAsync(x => x.IdChat == request.IdChat && x.IdGroupMember == request.IdPerson, cancellationToken);

        if (!isPersonExists) return Result<GetMessageResponse>.Failure(new ForbiddenAccessException("You don't have access to this chat!"));

        var messageInfos = request.IsSingleChat
            ? await dbContext.SingleMessages
                .Where(x => x.IdChat == request.IdChat)
                .OrderByDescending(x=>x.CreatedAt)
                .Skip(request.SkipMessage)
                .Take(40)
                .Select(x => new MessageInfoDTO
                {
                    IdSender = x.IdSender,
                    Message = x.Message,
                    CreatedAt = x.CreatedAt
                })
                .ToListAsync(cancellationToken)
            : await dbContext.GroupMessages
                .Where(x => x.IdChat == request.IdChat)
                .OrderByDescending(x=>x.CreatedAt)
                .Skip(request.SkipMessage)
                .Take(40)
                .Select(x => new MessageInfoDTO
                {
                    IdSender = x.IdSender,
                    Message = x.Message,
                    CreatedAt = x.CreatedAt
                })
                .ToListAsync(cancellationToken);

        return Result<GetMessageResponse>.Ok(new GetMessageResponse
        {
            IdChat = request.IdChat,
            Message = messageInfos
        });
    }
}