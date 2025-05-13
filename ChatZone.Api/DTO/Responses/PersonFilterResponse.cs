using ChatZone.Core.Models.Enums;

namespace ChatZone.DTO.Responses;

public class PersonFilterResponse
{
    public CountryList? Country { get; set; }
    public CityList? City { get; set; }
    public AgeList? Age { get; set; }
    public GenderList? Gender { get; set; }
    public LangList? NativeLang { get; set; }
    public LangList? LearnLang { get; set; }
}