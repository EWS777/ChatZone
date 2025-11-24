using ChatZone.Shared.DTOs;
using MediatR;

namespace ChatZone.Features.Messages.Get;

public class GetMessageRequest : IRequest<Result<GetMessageResponse>>
{
    public int IdPerson { get; set; }
    public int IdChat { get; set; }
    public bool IsSingleChat { get; set; }
    public int SkipMessage { get; set; }
}