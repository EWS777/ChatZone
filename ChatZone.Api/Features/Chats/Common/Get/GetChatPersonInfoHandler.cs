using ChatZone.Shared.Context;
using ChatZone.Shared.DTOs;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ChatZone.Features.Chats.Common.Get;

public class GetChatPersonInfoHandler(
    ChatZoneDbContext dbContext) : IRequestHandler<GetChatPersonInfoRequest, Result<GetChatPersonInfoResponse>>
{
    public async Task<Result<GetChatPersonInfoResponse>> Handle(GetChatPersonInfoRequest request, CancellationToken cancellationToken)
    {
        var singleChat = await dbContext.SingleChats
            .SingleOrDefaultAsync(x=> (x.IdFirstPerson == request.IdPerson || x.IdSecondPerson == request.IdPerson) 
                                  && x.FinishedAt == null, cancellationToken); 
        if (singleChat is not null)
        {
            var isSentMessage = await dbContext.SingleMessages.AnyAsync(x=>x.IdChat==singleChat.IdSingleChat && x.IdSender == request.IdPerson, cancellationToken);
            
            return Result<GetChatPersonInfoResponse>.Ok(new GetChatPersonInfoResponse
            {
                IdPerson = request.IdPerson,
                IdGroup = singleChat.IdSingleChat,
                IsSingleChat = true,
                IdPartnerPerson = request.IdPerson == singleChat.IdFirstPerson ? singleChat.IdSecondPerson : singleChat.IdFirstPerson,
                IsSentMessage = isSentMessage
            });
        }
        
        var groupChat = await dbContext.GroupMembers.SingleOrDefaultAsync(x=>x.IdGroupMember == request.IdPerson, cancellationToken);
        if (groupChat is not null)
        {
            var isSentMessage = await dbContext.GroupMessages.AnyAsync(x=>x.IdChat==groupChat.IdChat && x.IdSender == request.IdPerson, cancellationToken);
            
            return Result<GetChatPersonInfoResponse>.Ok(new GetChatPersonInfoResponse
            {
                IdPerson = request.IdPerson,
                IdGroup = groupChat.IdChat,
                IsSingleChat = false,
                IdPartnerPerson = null,
                IsSentMessage = isSentMessage
            });
        }
        
        return Result<GetChatPersonInfoResponse>.Ok(new GetChatPersonInfoResponse
        {
            IdPerson = request.IdPerson,
            IdGroup = null,
            IsSingleChat = null,
            IdPartnerPerson = null,
            IsSentMessage = null
        });
    }
}