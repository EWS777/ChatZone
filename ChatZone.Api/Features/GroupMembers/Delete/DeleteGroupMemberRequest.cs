using ChatZone.Core.Extensions;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ChatZone.Features.GroupMembers.Delete;

public class DeleteGroupMemberRequest : IRequest<Result<IActionResult>>
{
    public int IdPerson { get; set; }
}