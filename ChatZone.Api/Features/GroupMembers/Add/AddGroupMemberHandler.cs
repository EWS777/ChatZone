using ChatZone.Core.Extensions.Exceptions;
using ChatZone.Core.Models;
using ChatZone.Shared.Context;
using ChatZone.Shared.DTOs;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ChatZone.Features.GroupMembers.Add;

public class AddGroupMemberHandler(ChatZoneDbContext dbContext) : IRequestHandler<AddGroupMemberRequest, Result<bool>>
{
    public async Task<Result<bool>> Handle(AddGroupMemberRequest request, CancellationToken cancellationToken)
    {
        var groupChat = await dbContext.GroupChats.SingleOrDefaultAsync(x => x.IdGroupChat == request.IdGroup, cancellationToken);
        if (groupChat is null) return Result<bool>.Failure(new NotFoundException("Group chat is not found!"));
        
        var isPersonExists = await dbContext.GroupMembers.AnyAsync(x => x.IdChat == request.IdGroup && x.IdGroupMember == request.IdPerson, cancellationToken);
        if (isPersonExists) return Result<bool>.Ok(true);
        
        var isAnyActiveGroupChat = await dbContext.GroupMembers.AnyAsync(x => x.IdGroupMember == request.IdPerson, cancellationToken);
        if (isAnyActiveGroupChat) return Result<bool>.Failure(new ExistPersonException("You already have an active group chat!"));

        var isAnyActiveSingleChat = await dbContext.SingleChats.AnyAsync(x => (x.IdFirstPerson == request.IdPerson || x.IdSecondPerson == request.IdPerson) && x.FinishedAt == null, cancellationToken);
        if (isAnyActiveSingleChat) return Result<bool>.Failure(new ExistPersonException("You already have an active single chat!"));

        var isPersonBlocked = await dbContext.BlockedGroupMembers.AnyAsync(x => x.IdChat == request.IdGroup && x.IdBlockedPerson == request.IdPerson, cancellationToken);
        if (isPersonBlocked) return Result<bool>.Failure(new ForbiddenAccessException("You are blocked in this group!"));
        
        await dbContext.GroupMembers.AddAsync(new GroupMember
        {
            IdChat = request.IdGroup,
            IdGroupMember = request.IdPerson,
            IsAdmin = false,
            JoinedAt = DateTimeOffset.UtcNow
        }, cancellationToken);

        groupChat.UserCount += 1;
        
        await dbContext.SaveChangesAsync(cancellationToken);
        
        return Result<bool>.Ok(true);
    }
}