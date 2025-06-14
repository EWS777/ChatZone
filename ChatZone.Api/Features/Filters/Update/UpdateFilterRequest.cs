using ChatZone.Core.Extensions;
using ChatZone.Core.Models.Enums;
using MediatR;

namespace ChatZone.Features.Filters.Update;

public class UpdateFilterRequest : IRequest<Result<UpdateFilterResponse>>
{
    public int Id { get; set; }
    public ThemeList? ThemeList { get; set; }
    public CountryList? Country { get; set; }
    public CityList? City { get; set; }
    public AgeList? Age { get; set; }
    public GenderList? Gender { get; set; }
    public LangList? NativeLang { get; set; }
    public LangList? LearnLang { get; set; }
}