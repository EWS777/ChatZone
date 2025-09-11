using System.ComponentModel.DataAnnotations;
using ChatZone.Core.Extensions;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ChatZone.Features.GroupChats.Delete;

public class DeleteGroupChatRequest : IRequest<Result<IActionResult>>
{
    [Required(ErrorMessage = "Id group can not be null!")]
    public int IdGroup { get; set; }
    public int IdPerson { get; set; }
}