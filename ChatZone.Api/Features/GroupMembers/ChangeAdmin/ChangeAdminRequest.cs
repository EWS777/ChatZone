using ChatZone.Shared.DTOs;
using MediatR;

namespace ChatZone.Features.GroupMembers.ChangeAdmin;

public class ChangeAdminRequest : IRequest<Result<bool>>
{
    public int IdPerson { get; set; }
    public int IdNewAdminPerson { get; set; }
    public int IdGroup { get; set; }
}