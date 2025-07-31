using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ChatZone.Features.SingleMessages.Add;

public class AddSingleMessageRequest : IRequest<IActionResult>
{
    public required string Message { get; set; }
    public int IdSender { get; set; }
    public int IdChat { get; set; }
}