using ChatZone.Context;
using ChatZone.Core.Extensions;
using ChatZone.Core.Extensions.Exceptions;
using ChatZone.Core.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ChatZone.Features.ChatGroups.Create;

public class CreateGroupHandler(
    ChatZoneDbContext dbContext) : IRequestHandler<CreateGroupRequest, Result<int>>
{
    public async Task<Result<int>> Handle(CreateGroupRequest request, CancellationToken cancellationToken)
    {
        var exists = await dbContext.GroupMembers.AnyAsync(x => x.IdGroupMember == request.IdPerson, cancellationToken);
        if (exists) return Result<int>.Failure(new ExistPersonException("You already have a group!"));
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