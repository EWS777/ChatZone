using ChatZone.Shared.DTOs;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ChatZone.Features.Messages.Add;

public class AddMessageRequest : IRequest<Result<bool>>
{
    public required string Message { get; set; }
    public int IdSender { get; set; }
    public int IdChat { get; set; }
    public bool IsSingleChat { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
}