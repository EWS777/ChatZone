using ChatZone.Core.Extensions.Exceptions;
using ChatZone.Core.Models;
using ChatZone.Shared.Context;
using ChatZone.Shared.DTOs;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ChatZone.Features.Chats.GroupChats.Create;

public class CreateGroupChatHandler(
    ChatZoneDbContext dbContext) : IRequestHandler<CreateGroupChatRequest, Result<int>>
{
    public async Task<Result<int>> Handle(CreateGroupChatRequest request, CancellationToken cancellationToken)
    {
        var isAnyActiveGroupChat = await dbContext.GroupMembers.AnyAsync(x => x.IdGroupMember == request.IdPerson, cancellationToken);
        if (isAnyActiveGroupChat) return Result<int>.Failure(new ExistPersonException("You already have an active group chat!"));
        
        var isAnyActiveSingleChat = await dbContext.SingleChats.AnyAsync(x =>  (x.IdFirstPerson == request.IdPerson || x.IdSecondPerson == request.IdPerson) && x.FinishedAt == null, cancellationToken);
        if (isAnyActiveSingleChat) return Result<int>.Failure(new ExistPersonException("You already have an active single chat!"));
        
        var chat = new GroupChat
        {
            Title = request.Title,
            Country = request.Country,
            City = request.City,
            Age = request.Age,
            Lang = request.Lang,
            UserCount = 1,
            GroupMembers = new List<GroupMember>
            {
                new GroupMember
                {
                    IdGroupMember = request.IdPerson,
                    IsAdmin = true,
                    JoinedAt = DateTimeOffset.UtcNow
                }
            }
        };

        await dbContext.GroupChats.AddAsync(chat, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);
        return Result<int>.Ok(chat.IdGroupChat);
    }
}