using ChatZone.Core.Extensions.Exceptions;
using ChatZone.Core.Models;
using ChatZone.Shared.Context;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ChatZone.Features.Messages.Add;

public class AddMessageHandler(
    ChatZoneDbContext dbContext) : IRequestHandler<AddMessageRequest, bool>
{
    public async Task<bool> Handle(AddMessageRequest request, CancellationToken cancellationToken)
    {
        if (request.IsSingleChat)
        {
            var isPersonExists = await dbContext.SingleChats.AnyAsync(x => x.IdSingleChat == request.IdChat
                                                                           && (x.IdFirstPerson == request.IdSender || x.IdSecondPerson == request.IdSender), cancellationToken);
            if (!isPersonExists) throw new NotFoundException("User is not exists in this group!");
            
            var message = new SingleMessage
            {
                CreatedAt = request.CreatedAt,
                Message = request.Message,
                IdSender = request.IdSender,
                IdChat = request.IdChat
            };
            await dbContext.SingleMessages.AddAsync(message, cancellationToken);
        }
        else
        {
            var isPersonExists = await dbContext.GroupMembers.AnyAsync(
                x => x.IdChat == request.IdChat && x.IdGroupMember == request.IdSender, cancellationToken);
            if (!isPersonExists) throw new NotFoundException("User is not exists in this group!");
            var message = new GroupMessage
            {
                CreatedAt = request.CreatedAt,
                Message = request.Message,
                IdSender = request.IdSender,
                IdChat = request.IdChat
            };
            await dbContext.GroupMessages.AddAsync(message, cancellationToken);
        }
        await dbContext.SaveChangesAsync(cancellationToken);
        return true;
    }
}