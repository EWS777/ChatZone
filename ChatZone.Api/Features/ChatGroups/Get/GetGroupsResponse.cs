using ChatZone.Core.Models.Enums;

namespace ChatZone.Features.ChatGroups.Get;

public class GetGroupsResponse
{
    public int IdGroup { get; set; }
    public required string Title { get; set; }
    public CountryList? Country { get; set; }
    public CityList? City { get; set; }
    public AgeList? Age { get; set; }
    public LangList? Lang { get; set; }
}