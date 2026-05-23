using MediatR;

namespace ChatZone.Features.GroupMembers.CheckMember;

public class CheckMemberRequest : IRequest<bool>
{
    public int IdPerson { get; set; }
    public int IdChat { get; set; }
}