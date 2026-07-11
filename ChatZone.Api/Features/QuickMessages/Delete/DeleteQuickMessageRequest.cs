using ChatZone.Shared.DTOs;
using MediatR;

namespace ChatZone.Features.QuickMessages.Delete;

public class DeleteQuickMessageRequest : IRequest<Result<bool>>
{
    public int IdPerson { get; set; }
    public int IdMessage { get; set; }
}