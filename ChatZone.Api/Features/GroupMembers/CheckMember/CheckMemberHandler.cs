using ChatZone.Shared.Context;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ChatZone.Features.GroupMembers.CheckMember;

public class CheckMemberHandler(ChatZoneDbContext dbContext) : IRequestHandler<CheckMemberRequest, bool>
{
    public async Task<bool> Handle(CheckMemberRequest request, CancellationToken cancellationToken)
    {
        return await dbContext.GroupMembers.AnyAsync(
            x => x.IdChat == request.IdChat && x.IdGroupMember == request.IdPerson, cancellationToken);
    }
}