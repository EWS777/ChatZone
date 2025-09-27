using MediatR;

namespace ChatZone.Features.Chats.Common.GetActiveChat;

public class GetActiveChatRequest : IRequest<GetActiveChatResponse>
{
    public int IdPerson { get; set; }
}