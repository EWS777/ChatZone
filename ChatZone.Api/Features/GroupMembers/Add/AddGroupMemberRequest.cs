using ChatZone.Core.Extensions;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ChatZone.Features.GroupMembers.Add;

public class AddGroupMemberRequest : IRequest<Result<IActionResult>>
{
    public int IdPerson { get; set; }
    public int IdGroup { get; set; }
}