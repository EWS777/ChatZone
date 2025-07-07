using ChatZone.Core.Extensions;
using ChatZone.Core.Models.Enums;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ChatZone.Features.Search.Find;

public class FindPersonRequest : IRequest<Result<IActionResult>>
{
    public int IdPerson { get; set; }
    public ThemeList? ThemeList { get; set; }
    public CountryList? Country { get; set; }
    public CityList? City { get; set; }
    public AgeList? Age { get; set; }
    public GenderList? Gender { get; set; }
    public LangList? Lang { get; set; }
}