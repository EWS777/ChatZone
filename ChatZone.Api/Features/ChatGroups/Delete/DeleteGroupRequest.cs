using System.ComponentModel.DataAnnotations;
using ChatZone.Core.Extensions;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ChatZone.Features.ChatGroups.Delete;

public class DeleteGroupRequest : IRequest<Result<IActionResult>>
{
    [Required(ErrorMessage = "Id group can not be null!")]
    public int IdGroup { get; set; }
    public int IdPerson { get; set; }
}