using System.ComponentModel.DataAnnotations;
using ChatZone.Core.Extensions;
using MediatR;

namespace ChatZone.Features.ChatGroups.Get;

public class GetGroupRequest : IRequest<Result<GetGroupResponse>>
{
    public int IdPerson { get; set; }
    [Required(ErrorMessage = "Id group can not be null!")]
    public int IdGroup { get; set; }
}