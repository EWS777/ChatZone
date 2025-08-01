using ChatZone.Context;
using ChatZone.Core.Extensions;
using ChatZone.Core.Extensions.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ChatZone.Features.ChatGroups.Get;

public class GetGroupHandler(
    ChatZoneDbContext dbContext) : IRequestHandler<GetGroupRequest, Result<GetGroupResponse>>
{
    public async Task<Result<GetGroupResponse>> Handle(GetGroupRequest request, CancellationToken cancellationToken)
    {
        var group = await dbContext.GroupChats
            .AsNoTracking()
            .Where(x => x.IdGroupChat == request.IdGroup)
            .Select(x => new GetGroupResponse
            {
                IdGroup = x.IdGroupChat,
                Title = x.Title,
                Country = x.Country,
                City = x.City,
                Age = x.Age,
                Lang = x.Lang,
                IsAdmin = x.GroupMembers
                    .Where(q=>q.IdGroupMember == request.IdPerson)
                    .Select(q=>q.IsAdmin)
                    .SingleOrDefault()
            })
            .SingleOrDefaultAsync(cancellationToken);
        
        return group is null ? Result<GetGroupResponse>.Failure(new NotFoundException("Group is not found!")) : Result<GetGroupResponse>.Ok(group);
    }
}