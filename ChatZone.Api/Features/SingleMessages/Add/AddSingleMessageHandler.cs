using ChatZone.Context;
using ChatZone.Core.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ChatZone.Features.SingleMessages.Add;

public class AddSingleMessageHandler(
    ChatZoneDbContext dbContext) : IRequestHandler<AddSingleMessageRequest, IActionResult>
{
    public async Task<IActionResult> Handle(AddSingleMessageRequest request, CancellationToken cancellationToken)
    {
        var message = new SingleMessage
        {
            CreatedAt = DateTimeOffset.Now,
            Message = request.Message,
            IdSender = request.IdSender,
            IdChat = request.IdChat
        };

        await dbContext.SingleMessages.AddAsync(message, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);
        return new OkResult();
    }
}