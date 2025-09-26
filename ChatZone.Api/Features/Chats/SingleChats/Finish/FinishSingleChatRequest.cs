using MediatR;

namespace ChatZone.Features.Chats.SingleChats.Finish;

public class FinishSingleChatRequest : IRequest<Unit>
{
    public int IdChat { get; set; }
}