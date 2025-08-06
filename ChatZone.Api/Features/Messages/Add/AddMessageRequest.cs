using System.ComponentModel.DataAnnotations;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ChatZone.Features.Messages.Add;

public class AddMessageRequest : IRequest<IActionResult>
{
    [Required(ErrorMessage = "Message is required!")]
    [MaxLength(250, ErrorMessage = "Message max length is 250 characters!")]
    public required string Message { get; set; }
    public int IdSender { get; set; }
    [Required(ErrorMessage = "Id chat can not be null!")]
    public int IdChat { get; set; }
    public bool IsSingleChat { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
}