using ChatZone.Core.Extensions;
using ChatZone.Core.Models.Enums;
using MediatR;
namespace ChatZone.Features.ChatGroups.Create;

public class CreateGroupRequest : IRequest<Result<int>>
{
    public int IdPerson { get; set; }
    public string Title { get; set; }
    public CountryList? Country { get; set; }
    public CityList? City { get; set; }
    public AgeList? Age { get; set; }
    public LangList? Lang { get; set; }
}