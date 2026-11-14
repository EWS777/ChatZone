using ChatZone.Core.Models.Enums;

namespace ChatZone.Core.Models;

public class MatchQueue
{
    public int IdPerson { get; set; }
    public ThemeList? Theme { get; set; }
    public int? IdCountry { get; set; }
    public int? IdCity { get; set; }
    public AgeList? Age { get; set; }
    public GenderList? YourGender { get; set; }
    public GenderList? PartnerGender { get; set; }
    public int? IdLanguage { get; set; }
    public required string ConnectionId { get; set; }
    public required DateTimeOffset JoinedAt { get; set; }
    public bool IsRandomPartner { get; set; } = false;
}