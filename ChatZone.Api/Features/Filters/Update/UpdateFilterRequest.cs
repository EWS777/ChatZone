using ChatZone.Core.Models.Enums;
using ChatZone.Shared.DTOs;
using MediatR;

namespace ChatZone.Features.Filters.Update;

public class UpdateFilterRequest : IRequest<Result<bool>>
{
    public int IdPerson { get; set; }
    public ThemeList? Theme { get; set; }
    public int? IdCountry { get; set; }
    public int? IdCity { get; set; }
    public AgeList? Age { get; set; }
    public GenderList? YourGender { get; set; }
    public GenderList? PartnerGender { get; set; }
    public int? IdLanguage { get; set; }
    public bool IsFindRandomPerson { get; set; }
}