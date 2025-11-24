using ChatZone.Shared.DTOs;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ChatZone.Features.QuickMessages.Delete;

public class DeleteQuickMessageRequest : IRequest<Result<IActionResult>>
{
    public int IdPerson { get; set; }
    public int IdMessage { get; set; }
}