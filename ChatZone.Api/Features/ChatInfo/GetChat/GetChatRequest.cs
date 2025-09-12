using MediatR;

namespace ChatZone.Features.ChatInfo.GetChat;

public class GetChatRequest : IRequest<int?>
{
    public int IdPerson { get; set; }
}