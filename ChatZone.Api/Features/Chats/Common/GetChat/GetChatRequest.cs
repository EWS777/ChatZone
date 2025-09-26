using MediatR;

namespace ChatZone.Features.Chats.Common.GetChat;

public class GetChatRequest : IRequest<int?>
{
    public int IdPerson { get; set; }
}