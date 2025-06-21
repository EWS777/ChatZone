using System.ComponentModel.DataAnnotations;
using ChatZone.Core.Models.Enums;

namespace ChatZone.Core.Models;

public class Person
{
    public int IdPerson { get; set; }
    public required PersonRole Role { get; set; }
    public required string Username { get; set; }
    [EmailAddress]
    public required string Email { get; set; }
    public required string Password { get; set; }
    public required string Salt { get; set; }
    public required string RefreshToken { get; set; }
    public required DateTimeOffset RefreshTokenExp { get; set; }
    
    public LangList LangMenu { get; set; }
    public bool IsDarkTheme { get; set; }
    public bool IsFindByProfile { get; set; }
    
    public ThemeList? ThemeList { get; set; }
    public CountryList? Country { get; set; }
    public CityList? City { get; set; }
    public AgeList? Age { get; set; }
    public GenderList? Gender { get; set; }
    public LangList? NativeLang { get; set; }
    public LangList? LearnLang { get; set; }
    public string? EmailConfirmToken { get; set; }
    public DateTimeOffset? EmailConfirmTokenExp { get; set; }
    
    public ICollection<QuickMessage> QuickMessages { get; set; } = new List<QuickMessage>();
    public ICollection<BlockedPerson> BlockerPeoples { get; set; } = new List<BlockedPerson>();
    public ICollection<BlockedPerson> BlockedPeoples { get; set; } = new List<BlockedPerson>();
    public SingleChat FirstPerson { get; set; }
    public SingleChat SecondPerson { get; set; }
    public ICollection<SingleMessage> SingleMessages { get; set; } = new List<SingleMessage>();
    public ICollection<Report> Reporter { get; set; } = new List<Report>();
    public ICollection<Report> Reported { get; set; } = new List<Report>();
    public ICollection<GroupMessage> GroupMessages { get; set; } = new List<GroupMessage>();
    public GroupMember GroupMember { get; set; }
}