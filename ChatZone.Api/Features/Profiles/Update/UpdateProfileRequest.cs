using System.ComponentModel.DataAnnotations;
using ChatZone.Shared.DTOs;
using MediatR;

namespace ChatZone.Features.Profiles.Update;

public class UpdateProfileRequest : IRequest<Result<UpdateProfileResponse>>
{
    public int IdPerson { get; set; }
    [Required(ErrorMessage = "Username is required!")]
    [MinLength(8,ErrorMessage = "Username min length is 8 characters")]
    [MaxLength(20,ErrorMessage = "Username max length is 20 characters")]
    [RegularExpression("^[^@]*$", ErrorMessage = "Username can not contains '@'!")]
    public required string Username { get; set; }
    public bool IsFindByProfile { get; set; }
}