using ChatZone.Shared.DTOs;
using MediatR;

namespace ChatZone.Features.Chats.SingleChats.Finish;

public class FinishSingleChatRequest : IRequest<Result<bool>>
{
    public int IdChat { get; set; }
    public int IdPerson { get; set; }
}