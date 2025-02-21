using ChatZone.Core.Models.Enums;

namespace ChatZone.Core.Models;

public class PersonData
{
    public required string Username { get; set; }
    public required string Email { get; set; }
    public required string Password { get; set; }
    public CountryList? Country { get; set; }
    public CityList? City { get; set; }
    public AgeList? Age { get; set; }
    public GenderList? Gender { get; set; }
    public LangList? NativeLang { get; set; }
    public LangList? LearnLang { get; set; }
    public int PersonDataId { get; set; }
    public required Person Person { get; set; }
}