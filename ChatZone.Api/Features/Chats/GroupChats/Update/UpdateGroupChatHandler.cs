using ChatZone.Core.Extensions.Exceptions;
using ChatZone.Shared.Context;
using ChatZone.Shared.DTOs;
using ChatZone.Shared.Hubs;
using MediatR;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace ChatZone.Features.Chats.GroupChats.Update;

public class UpdateGroupChatHandler(
    ChatZoneDbContext dbContext,
    IHubContext<ChatZoneHub> hubContext) : IRequestHandler<UpdateGroupChatRequest, Result<UpdateGroupChatResponse>>
{
    public async Task<Result<UpdateGroupChatResponse>> Handle(UpdateGroupChatRequest request,
        CancellationToken cancellationToken)
    {
        var isAdmin = await dbContext.GroupMembers
            .Where(x => x.IdChat == request.IdGroup && x.IdGroupMember == request.IdPerson)
            .Select(x => x.IsAdmin)
            .SingleOrDefaultAsync(cancellationToken);

        if (!isAdmin) return Result<UpdateGroupChatResponse>.Failure(new ForbiddenAccessException("You are not an owner of this group!"));


        var group = await dbContext.GroupChats.SingleOrDefaultAsync(x => x.IdGroupChat == request.IdGroup, cancellationToken);
        if(group is null) return Result<UpdateGroupChatResponse>.Failure(new NotFoundException("Group is not found!"));

        group.Title = request.Title;
        group.Country = request.Country;
        group.City = request.City;
        group.Age = request.Age;
        group.Lang = request.Lang;

        dbContext.GroupChats.Update(group);
        await dbContext.SaveChangesAsync(cancellationToken);
        
        var result = new UpdateGroupChatResponse
        {
            IdGroup = group.IdGroupChat,
            Title = group.Title,
            Country = group.Country,
            City = group.City,
            Age = group.Age,
            Lang = group.Lang
        };
        
        await hubContext.Clients.Group(group.IdGroupChat.ToString()).SendAsync("UpdateGroupChatParam", result, cancellationToken);
        return Result<UpdateGroupChatResponse>.Ok(result);
    }
}