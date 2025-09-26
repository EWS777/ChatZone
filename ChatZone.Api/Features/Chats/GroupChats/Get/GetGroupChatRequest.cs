using System.ComponentModel.DataAnnotations;
using ChatZone.Core.Extensions;
using MediatR;

namespace ChatZone.Features.Chats.GroupChats.Get;

public class GetGroupChatRequest : IRequest<Result<GetGroupChatResponse>>
{
    public int IdPerson { get; set; }
    [Required(ErrorMessage = "Id group can not be null!")]
    public int IdGroup { get; set; }
}