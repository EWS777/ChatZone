using ChatZone.Core.Models.Enums;
using ChatZone.Shared.DTOs;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ChatZone.Features.Search.Find;

public class FindPersonRequest : IRequest<Result<IActionResult>>
{
    public int IdPerson { get; set; }
    public string ConnectionId { get; set; } = string.Empty;
    public ThemeList? Theme { get; set; }
    public CountryList? Country { get; set; }
    public CityList? City { get; set; }
    public AgeList? Age { get; set; }
    public GenderList? YourGender { get; set; }
    public GenderList? PartnerGender { get; set; }
    public LangList? Language { get; set; }
    public bool IsSearchAgain { get; set; } = false;
}