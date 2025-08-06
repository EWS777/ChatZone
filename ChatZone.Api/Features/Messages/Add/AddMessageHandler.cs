using ChatZone.Context;
using ChatZone.Core.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ChatZone.Features.Messages.Add;

public class AddMessageHandler(
    ChatZoneDbContext dbContext) : IRequestHandler<AddMessageRequest, IActionResult>
{
    public async Task<IActionResult> Handle(AddMessageRequest request, CancellationToken cancellationToken)
    {
        if (request.IsSingleChat)
        {
            var message = new SingleMessage
            {
                CreatedAt = request.CreatedAt,
                Message = request.Message,
                IdSender = request.IdSender,
                IdChat = request.IdChat
            };
            await dbContext.SingleMessages.AddAsync(message, cancellationToken);
        }
        
        if (!request.IsSingleChat)
        {
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
        return new OkResult();
    }
}