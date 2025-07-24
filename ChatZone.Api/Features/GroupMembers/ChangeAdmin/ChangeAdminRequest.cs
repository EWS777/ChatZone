using ChatZone.Core.Extensions;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ChatZone.Features.GroupMembers.ChangeAdmin;

public class ChangeAdminRequest : IRequest<Result<IActionResult>>
{
    public int IdPerson { get; set; }
    public int IdNewAdminPerson { get; set; }
    public int IdGroup { get; set; }
}