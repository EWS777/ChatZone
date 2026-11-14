using ChatZone.Core.Models.Enums;
using ChatZone.Shared.DTOs;

namespace ChatZone.Features.Filters.Get;

public class GetFilterResponse
{
    public ThemeList? Theme { get; set; }
    public NamedItemDto? Country { get; set; }
    public NamedItemDto? City { get; set; }
    public AgeList? Age { get; set; }
    public GenderList? YourGender { get; set; }
    public GenderList? PartnerGender { get; set; }
    public NamedItemDto? Language { get; set; }
}
