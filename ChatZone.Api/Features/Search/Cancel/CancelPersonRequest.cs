using ChatZone.Core.Extensions;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ChatZone.Features.Search.Cancel;

public class CancelPersonRequest : IRequest<Result<IActionResult>>
{
    public int IdPerson { get; init; }
}