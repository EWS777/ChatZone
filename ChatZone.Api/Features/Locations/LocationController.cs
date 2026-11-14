using ChatZone.Features.Locations.GetCities;
using ChatZone.Features.Locations.GetCountries;
using ChatZone.Features.Locations.GetLanguages;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ChatZone.Features.Locations;

[ApiController]
[Authorize(Roles = "User")]
[Route("[controller]")]
public class LocationController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    [Route("countries")]
    public async Task<ActionResult<List<GetCountriesResponse>>> GetCountries(CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new GetCountriesRequest(), cancellationToken);
        return result.Match(x => x, x => throw x);
    }

    [HttpGet]
    [Route("countries/{idCountry:int}/cities")]
    public async Task<ActionResult<List<GetCitiesResponse>>> GetCities(int idCountry, [FromQuery] GetCitiesRequest request, CancellationToken cancellationToken)
    {
        request.IdCountry = idCountry;
        var result = await mediator.Send(request, cancellationToken);
        return result.Match(x => x, x => throw x);
    }

    [HttpGet]
    [Route("languages")]
    public async Task<ActionResult<List<GetLanguagesResponse>>> GetLanguages(CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new GetLanguagesRequest(), cancellationToken);
        return result.Match(x => x, x => throw x);
    }
}
