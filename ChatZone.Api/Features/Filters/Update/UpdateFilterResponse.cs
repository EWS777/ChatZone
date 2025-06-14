using ChatZone.Core.Models.Enums;

namespace ChatZone.Features.Filters.Update;

public class UpdateFilterResponse
{
    public ThemeList? ThemeList { get; set; }
    public CountryList? Country { get; set; }
    public CityList? City { get; set; }
    public AgeList? Age { get; set; }
    public GenderList? Gender { get; set; }
    public LangList? NativeLang { get; set; }
    public LangList? LearnLang { get; set; }
}