using ChatZone.Core.Extensions;
using MediatR;

namespace ChatZone.Features.Chats.Common.Get;

public class GetChatPersonInfoRequest : IRequest<Result<GetChatPersonInfoResponse>>
{
    public int IdPerson { get; set; }
}