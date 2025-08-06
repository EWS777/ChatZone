using MediatR;

namespace ChatZone.Features.GroupMembers.ChangeAdminAuto;

public class ChangeAdminAutoRequest : IRequest<Unit>
{
    public int IdChat { get; init; }
    public int IdPersonAdmin { get; init; }
}