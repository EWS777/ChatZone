using ChatZone.Core.Models.Enums;

namespace ChatZone.Features.Filters.Get;

public class GetFilterResponse
{
    public ThemeList? ThemeList { get; set; }
    public CountryList? Country { get; set; }
    public CityList? City { get; set; }
    public AgeList? Age { get; set; }
    public GenderList? Gender { get; set; }
    public LangList? NativeLang { get; set; }
    public LangList? LearnLang { get; set; }
}