using System.ComponentModel.DataAnnotations;
using ChatZone.Core.Extensions;
using ChatZone.Core.Models.Enums;
using MediatR;
namespace ChatZone.Features.ChatGroups.Create;

public class CreateGroupRequest : IRequest<Result<int>>
{
    public int IdPerson { get; set; }
    [MaxLength(25, ErrorMessage = "Title max length is 25 characters")]
    public required string Title { get; set; }
    public CountryList? Country { get; set; }
    public CityList? City { get; set; }
    public AgeList? Age { get; set; }
    public LangList? Lang { get; set; }
}