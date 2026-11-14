using ChatZone.Shared.DTOs;
using MediatR;

namespace ChatZone.Features.Locations.GetCountries;

public class GetCountriesRequest : IRequest<Result<List<GetCountriesResponse>>>;
