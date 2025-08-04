using ChatZone.Core.Extensions;
using MediatR;

namespace ChatZone.Features.QuickMessages.GetList;

public class GetQuickMessageListRequest : IRequest<Result<List<GetQuickMessageListResponse>>>
{
    public int IdPerson { get; init; }
}