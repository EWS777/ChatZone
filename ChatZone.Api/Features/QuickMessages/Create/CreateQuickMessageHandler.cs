using System.Security.Claims;
using ChatZone.Context;
using ChatZone.Core.Extensions;
using ChatZone.Core.Extensions.Exceptions;
using ChatZone.Core.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ChatZone.Features.QuickMessages.Create;

public class CreateQuickMessageHandler(
    ChatZoneDbContext dbContext,
    IHttpContextAccessor httpContextAccessor)
    : IRequestHandler<CreateQuickMessageRequest, Result<CreateQuickMessageResponse>>
{
    public async Task<Result<CreateQuickMessageResponse>> Handle(CreateQuickMessageRequest request, CancellationToken cancellationToken)
    {
        //delete http and add id in controller to DTO?
        var id = httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);
        var userId = int.Parse(id!);
        
        var quickMessage = new QuickMessage
        {
            Message = request.Message,
            IdPerson = userId
        };
        
        await dbContext.QuickMessages.AddAsync(quickMessage, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);

        return Result<CreateQuickMessageResponse>.Ok(new CreateQuickMessageResponse
        {
            IdQuickMessage = quickMessage.IdQuickMessage,
            Message = quickMessage.Message
        });
    }
}