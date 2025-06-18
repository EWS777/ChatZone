using System.ComponentModel.DataAnnotations;
using ChatZone.Core.Extensions;
using MediatR;

namespace ChatZone.Features.QuickMessages.Update;

public class UpdateQuickMessageRequest : IRequest<Result<UpdateQuickMessageResponse>>
{
    public int IdQuickMessage { get; set; }
    [Required]
    [MaxLength(250)]
    public required string Message { get; set; }
    public int IdPerson { get; set; }
}