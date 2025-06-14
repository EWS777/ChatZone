using ChatZone.Core.Extensions;
using MediatR;

namespace ChatZone.Features.QuickMessages.Create;

public class CreateQuickMessageRequest : IRequest<Result<CreateQuickMessageResponse>>
{
    public required string Message { get; set; }
}