using MediatR;

namespace ChatZone.Features.Messages.GetStatus;

public class GetStatusRequest : IRequest<bool>
{
    public int IdPerson { get; set; }
    public int IdChat { get; set; }
    public bool IsSingleChat { get; set; }
}