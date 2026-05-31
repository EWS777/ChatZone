using ChatZone.Shared.DTOs;
using MediatR;

namespace ChatZone.Features.Chats.Common.GetActiveChat;

public class GetActiveChatRequest : IRequest<Result<GetActiveChatResponse>>
{
    public int IdPerson { get; set; }
}