using ChatZone.Core.Models.Enums;

namespace ChatZone.Core.Models;

public class MatchQueue
{
    public int IdPerson { get; set; }
    public ThemeList? ThemeList { get; set; }
    public CountryList? Country { get; set; }
    public CityList? City { get; set; }
    public AgeList? Age { get; set; }
    public GenderList? Gender { get; set; }
    public LangList? Lang { get; set; }
}