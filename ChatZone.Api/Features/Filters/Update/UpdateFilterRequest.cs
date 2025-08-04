using ChatZone.Core.Extensions;
using ChatZone.Core.Models.Enums;
using MediatR;

namespace ChatZone.Features.Filters.Update;

public class UpdateFilterRequest : IRequest<Result<UpdateFilterResponse>>
{
    public int IdPerson { get; set; }
    public ThemeList? Theme { get; set; }
    public CountryList? Country { get; set; }
    public CityList? City { get; set; }
    public AgeList? Age { get; set; }
    public GenderList? YourGender { get; set; }
    public GenderList? PartnerGender { get; set; }
    public LangList? Language { get; set; }
}