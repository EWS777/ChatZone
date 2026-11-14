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
        
        var isAnyActiveSingleChat = await dbContext.SingleChats.AnyAsync(x => (x.IdFirstPerson == request.IdPerson || x.IdSecondPerson == request.IdPerson) && x.FinishedAt == null, cancellationToken);
        if (isAnyActiveSingleChat) return Result<int>.Failure(new ExistPersonException("You already have an active single chat!"));
        
        var locationError = await dbContext.ValidateLocationAsync(request.IdCountry, request.IdCity, request.IdLanguage, cancellationToken);
        if (locationError is not null) return Result<int>.Failure(locationError);

        var chat = new GroupChat
        {
            Title = request.Title,
            IdCountry = request.IdCountry,
            IdCity = request.IdCity,
            Age = request.Age,
            IdLanguage = request.IdLanguage,
            UserCount = 1,
            CreatedAt = DateTimeOffset.UtcNow,
            GroupMembers = new List<GroupMember>
            {
                new()
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