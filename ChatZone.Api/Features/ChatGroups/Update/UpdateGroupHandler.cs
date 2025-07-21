using ChatZone.Context;
using ChatZone.Core.Extensions;
using ChatZone.Core.Extensions.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ChatZone.Features.ChatGroups.Update;

public class UpdateGroupHandler(
    ChatZoneDbContext dbContext) : IRequestHandler<UpdateGroupRequest, Result<UpdateGroupResponse>>
{
    public async Task<Result<UpdateGroupResponse>> Handle(UpdateGroupRequest request,
        CancellationToken cancellationToken)
    {
        var isAdmin = await dbContext.GroupMembers
            .Where(x => x.IdChat == request.IdGroup && x.IdGroupMember == request.IdPerson)
            .Select(x => x.IsAdmin)
            .SingleOrDefaultAsync(cancellationToken);

        if (!isAdmin) return Result<UpdateGroupResponse>.Failure(new ForbiddenAccessException("You are not an owner of this group!"));


        var group = await dbContext.GroupChats.SingleOrDefaultAsync(x => x.IdGroupChat == request.IdGroup, cancellationToken);
        if(group is null) return Result<UpdateGroupResponse>.Failure(new NotFoundException("Group is not found!"));

        group.Title = request.Title;
        group.Country = request.Country;
        group.City = request.City;
        group.Age = request.Age;
        group.Lang = request.Lang;

        dbContext.GroupChats.Update(group);
        await dbContext.SaveChangesAsync(cancellationToken);
        
        return Result<UpdateGroupResponse>.Ok(new UpdateGroupResponse
        {
            IdGroup = group.IdGroupChat,
            Title = group.Title,
            Country = group.Country,
            City = group.City,
            Age = group.Age,
            Lang = group.Lang
        });
    }
}