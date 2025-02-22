using ChatZone.Core.Models.Enums;

namespace ChatZone.Core.Models;

public class PersonData
{
    public CountryList? Country { get; set; }
    public CityList? City { get; set; }
    public AgeList? Age { get; set; }
    public GenderList? Gender { get; set; }
    public LangList? NativeLang { get; set; }
    public LangList? LearnLang { get; set; }
    public int PersonDataId { get; set; }
    public Person Person { get; set; }
}