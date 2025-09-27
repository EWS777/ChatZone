using MediatR;

namespace ChatZone.Features.Chats.Common.GetActiveChat;

public class GetActiveChatRequest : IRequest<int?>
{
    public int IdPerson { get; set; }
}