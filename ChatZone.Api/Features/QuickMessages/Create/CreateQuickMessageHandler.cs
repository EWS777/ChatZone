using System.Security.Claims;
using ChatZone.Context;
using ChatZone.Core.Extensions;
using ChatZone.Core.Extensions.Exceptions;
using ChatZone.Core.Models;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ChatZone.Features.QuickMessages.Create;

public class CreateQuickMessageHandler(
    ChatZoneDbContext dbContext,
    IValidator<CreateQuickMessageRequest> validator,
    IHttpContextAccessor httpContextAccessor)
    : IRequestHandler<CreateQuickMessageRequest, Result<CreateQuickMessageResponse>>
{
    public async Task<Result<CreateQuickMessageResponse>> Handle(CreateQuickMessageRequest request, CancellationToken cancellationToken)
    {
        var validation = await validator.ValidateAsync(request, cancellationToken);
        if (!validation.IsValid) return Result<CreateQuickMessageResponse>.Failure(validation.Errors.ToList());
        
        //delete http and add id in controller to DTO?
        var id = httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);

        var userId = int.Parse(id!);
        
        var isPersonExists = await dbContext.Persons.AnyAsync(x=>x.IdPerson==userId, cancellationToken);
        if (!isPersonExists) return Result<CreateQuickMessageResponse>.Failure(new NotFoundException("User is not found!"));
        
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