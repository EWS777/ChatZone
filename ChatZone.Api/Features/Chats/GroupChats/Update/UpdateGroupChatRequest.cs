using System.ComponentModel.DataAnnotations;
using ChatZone.Core.Models.Enums;
using ChatZone.Shared.DTOs;
using MediatR;

namespace ChatZone.Features.Chats.GroupChats.Update;

public class UpdateGroupChatRequest : IRequest<Result<UpdateGroupChatResponse>>
{
    public int IdPerson { get; set; }
    public int IdGroup { get; set; }
    [Required(ErrorMessage = "Title is required!")]
    [MinLength(1,ErrorMessage = "Title min length is 1 characters")]
    [MaxLength(50,ErrorMessage = "Title max length is 50 characters")]
    public required string Title { get; set; }
    public CountryList? Country { get; set; }
    public CityList? City { get; set; }
    public AgeList? Age { get; set; }
    public LangList? Lang { get; set; }
}