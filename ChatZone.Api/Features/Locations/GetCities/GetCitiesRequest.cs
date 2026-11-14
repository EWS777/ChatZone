using ChatZone.Shared.DTOs;
using MediatR;

namespace ChatZone.Features.Locations.GetCities;

public class GetCitiesRequest : IRequest<Result<List<GetCitiesResponse>>>
{
    public int IdCountry { get; set; }
    public string? Search { get; set; }
    public int Take { get; set; } = 20;
}
