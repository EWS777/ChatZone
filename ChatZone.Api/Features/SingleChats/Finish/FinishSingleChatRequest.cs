using MediatR;

namespace ChatZone.Features.SingleChats.Finish;

public class FinishSingleChatRequest : IRequest<Unit>
{
    public int IdChat { get; set; }
}