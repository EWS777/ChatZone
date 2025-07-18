using ChatZone.Context;
using ChatZone.Core.Extensions;
using ChatZone.Core.Extensions.Exceptions;
using ChatZone.Core.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ChatZone.Features.ChatGroups.Create;

public class CreateGroupHandler(
    ChatZoneDbContext dbContext) : IRequestHandler<CreateGroupRequest, Result<IActionResult>>
{
    public async Task<Result<IActionResult>> Handle(CreateGroupRequest request, CancellationToken cancellationToken)
    {
        var exists = await dbContext.ChatMembers.AnyAsync(x => x.IdGroupMember == request.IdPerson, cancellationToken);
        if (exists) return Result<IActionResult>.Failure(new ExistPersonException("You already have a group!"));
        var chat = new GroupChat
        {
            Title = request.Title,
            Country = request.Country,
            City = request.City,
            Age = request.Age,
            Lang = request.Lang,
            ChatMembers = new List<GroupMember>
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
        return Result<IActionResult>.Ok(new OkObjectResult(new {message = "Group has created successfully!"}));
    }
}