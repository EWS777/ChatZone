using System.ComponentModel.DataAnnotations;
using ChatZone.Shared.DTOs;
using MediatR;

namespace ChatZone.Features.QuickMessages.Create;

public class CreateQuickMessageRequest : IRequest<Result<CreateQuickMessageResponse>>
{
    [Required(ErrorMessage = "Message can't be empty!")]
    [MaxLength(50, ErrorMessage = "Message can't be longer than 50 characters!")]
    public required string Message { get; set; }
    public int IdPerson { get; set; }
}