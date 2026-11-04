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
            .AsNoTracking()
            .Where(x => (x.IdFirstPerson == request.IdPerson || x.IdSecondPerson == request.IdPerson) && x.FinishedAt == null)
            .Select(x => new
            {
                x.IdSingleChat,
                x.IdFirstPerson,
                x.IdSecondPerson,
                FirstUsername = x.FirstPerson.Username,
                SecondUsername = x.SecondPerson.Username,
                x.Theme,
                x.Country,
                x.City,
                x.Age,
                x.Language
            })
            .SingleOrDefaultAsync(cancellationToken);
        if (singleChat is not null)
        {
            var isSentMessage = await dbContext.SingleMessages.AnyAsync(x => x.IdChat == singleChat.IdSingleChat && x.IdSender == request.IdPerson, cancellationToken);
            
            var isFirst = request.IdPerson == singleChat.IdFirstPerson;
            var hasSearch = singleChat.Theme != null || singleChat.Country != null || singleChat.City != null || singleChat.Age != null || singleChat.Language != null;

            return Result<GetChatPersonInfoResponse>.Ok(new GetChatPersonInfoResponse
            {
                IdPerson = request.IdPerson,
                IdGroup = singleChat.IdSingleChat,
                IsSingleChat = true,
                IdPartnerPerson = isFirst ? singleChat.IdSecondPerson : singleChat.IdFirstPerson,
                IsSentMessage = isSentMessage,
                PartnerUsername = isFirst ? singleChat.SecondUsername : singleChat.FirstUsername,
                Search = !hasSearch ? null : new ChatSearchInfo
                {
                    Theme = (int?)singleChat.Theme,
                    Country = (int?)singleChat.Country,
                    City = (int?)singleChat.City,
                    Age = (int?)singleChat.Age,
                    Language = (int?)singleChat.Language
                }
            });
        }

        var groupChat = await dbContext.GroupMembers
            .AsNoTracking()
            .Where(x => x.IdGroupMember == request.IdPerson)
            .Select(x => new
            {
                x.IdChat
            })
            .SingleOrDefaultAsync(cancellationToken);
        if (groupChat is not null)
        {
            var isSentMessage = await dbContext.GroupMessages.AnyAsync(x => x.IdChat == groupChat.IdChat && x.IdSender == request.IdPerson, cancellationToken);
            
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