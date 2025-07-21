using ChatZone.Core.Extensions;
using ChatZone.Core.Models.Enums;
using MediatR;

namespace ChatZone.Features.ChatGroups.Update;

public class UpdateGroupRequest : IRequest<Result<UpdateGroupResponse>>
{
    public int IdPerson { get; set; }
    public int IdGroup { get; set; }
    public required string Title { get; set; }
    public CountryList? Country { get; set; }
    public CityList? City { get; set; }
    public AgeList? Age { get; set; }
    public LangList? Lang { get; set; }
}