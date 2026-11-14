using ChatZone.Shared.DTOs;
using MediatR;

namespace ChatZone.Features.Locations.GetLanguages;

public class GetLanguagesRequest : IRequest<Result<List<GetLanguagesResponse>>>;
