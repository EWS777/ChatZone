using ChatZone.Core.Models.Enums;

namespace ChatZone.Core.Models;

public class MatchQueue
{
    public int IdPerson { get; set; }
    public ThemeList? Theme { get; set; }
    public CountryList? Country { get; set; }
    public CityList? City { get; set; }
    public AgeList? Age { get; set; }
    public GenderList? YourGender { get; set; }
    public GenderList? PartnerGender { get; set; }
    public LangList? Language { get; set; }
    public required string ConnectionId { get; set; }
}