using ChatZone.Core.Models.Enums;

namespace ChatZone.Core.Models;

public class MatchQueue
{
    public int IdPerson { get; set; }
    public ThemeList? Theme { get; set; }
    public CountryList? Country { get; set; }
    public CityList? City { get; set; }
    public AgeList? Age { get; set; }
    public required GenderList YourGender { get; set; }
    public required GenderList PartnerGender { get; set; }
    public required LangList Language { get; set; }
    public required string ConnectionId { get; set; }
    public required DateTimeOffset JoinedAt { get; set; }
    public bool IsRandomPartner { get; set; } = false;
}