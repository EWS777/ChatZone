using ChatZone.Core.Models.Enums;

namespace ChatZone.DTO.Responses;

public class PersonFilterResponse
{
    public required CountryList? Country { get; set; }
    public required CityList? City { get; set; }
    public required AgeList? Age { get; set; }
    public required GenderList? Gender { get; set; }
    public required LangList? NativeLang { get; set; }
    public required LangList? LearnLang { get; set; }
}