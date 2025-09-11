using ChatZone.Context;
using ChatZone.Core.Extensions;
using ChatZone.Core.Extensions.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ChatZone.Features.GroupChats.Get;

public class GetGroupChatHandler(
    ChatZoneDbContext dbContext) : IRequestHandler<GetGroupChatRequest, Result<GetGroupChatResponse>>
{
    public async Task<Result<GetGroupChatResponse>> Handle(GetGroupChatRequest request, CancellationToken cancellationToken)
    {
        var group = await dbContext.GroupChats
            .AsNoTracking()
            .Where(x => x.IdGroupChat == request.IdGroup)
            .Select(x => new GetGroupChatResponse
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
        
        return group is null ? Result<GetGroupChatResponse>.Failure(new NotFoundException("Group is not found!")) : Result<GetGroupChatResponse>.Ok(group);
    }
}