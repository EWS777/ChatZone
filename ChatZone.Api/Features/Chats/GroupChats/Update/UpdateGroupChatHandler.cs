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
        var isAdmin = await dbContext.GroupMembers.AnyAsync(x => x.IdChat == request.IdGroup && x.IdGroupMember == request.IdPerson && x.IsAdmin, cancellationToken);
        if (!isAdmin) return Result<UpdateGroupChatResponse>.Failure(new ForbiddenAccessException("You are not an owner of this group!"));

        var group = await dbContext.GroupChats.SingleOrDefaultAsync(x => x.IdGroupChat == request.IdGroup, cancellationToken);
        if (group is null) return Result<UpdateGroupChatResponse>.Failure(new NotFoundException("Group is not found!"));

        var locationError = await dbContext.ValidateLocationAsync(request.IdCountry, request.IdCity, request.IdLanguage, cancellationToken);
        if (locationError is not null) return Result<UpdateGroupChatResponse>.Failure(locationError);

        group.Title = request.Title;
        group.IdCountry = request.IdCountry;
        group.IdCity = request.IdCity;
        group.Age = request.Age;
        group.IdLanguage = request.IdLanguage;

        await dbContext.SaveChangesAsync(cancellationToken);
        
        var result = await dbContext.GroupChats
            .AsNoTracking()
            .Where(x => x.IdGroupChat == group.IdGroupChat)
            .Select(x => new UpdateGroupChatResponse
            {
                IdGroup = x.IdGroupChat,
                Title = x.Title,
                Country = x.Country == null ? null : new NamedItemDto { Id = x.Country.IdCountry, Name = x.Country.Name },
                City = x.City == null ? null : new NamedItemDto { Id = x.City.IdCity, Name = x.City.Name },
                Age = x.Age,
                Language = x.Language == null ? null : new NamedItemDto { Id = x.Language.IdLanguage, Name = x.Language.Name }
            })
            .SingleAsync(cancellationToken);
        
        await hubContext.Clients.Group(group.IdGroupChat.ToString()).SendAsync("UpdateGroupChatParam", result, cancellationToken);
        return Result<UpdateGroupChatResponse>.Ok(result);
    }
}