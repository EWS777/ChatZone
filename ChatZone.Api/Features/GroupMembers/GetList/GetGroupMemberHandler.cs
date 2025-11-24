using ChatZone.Core.Extensions.Exceptions;
using ChatZone.Shared.Context;
using ChatZone.Shared.DTOs;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ChatZone.Features.GroupMembers.GetList;

public class GetGroupMemberHandler(
    ChatZoneDbContext dbContext) : IRequestHandler<GetGroupMemberRequest, Result<List<GetGroupMemberResponse>>>
{
    public async Task<Result<List<GetGroupMemberResponse>>> Handle(GetGroupMemberRequest request, CancellationToken cancellationToken)
    {
        var isAdmin = await dbContext.GroupMembers
            .Where(x => x.IdGroupMember == request.IdPerson)
            .Select(x => x.IsAdmin)
            .SingleOrDefaultAsync(cancellationToken);
        if(!isAdmin)
            return Result<List<GetGroupMemberResponse>>.Failure(new ForbiddenAccessException("You are not an admin of this group!"));

        var groupList = await dbContext.GroupMembers
            .AsNoTracking()
            .Where(x => x.IdChat == request.IdGroup && x.IdGroupMember != request.IdPerson)
            .Include(x => x.Person)
            .Select(x => new GetGroupMemberResponse
            {
                IdPerson = x.IdGroupMember,
                Username = x.Person.Username
            })
            .ToListAsync(cancellationToken);
        
        return Result<List<GetGroupMemberResponse>>.Ok(groupList);
    }
}