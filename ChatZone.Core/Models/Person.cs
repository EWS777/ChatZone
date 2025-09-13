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
    
    public ThemeList? Theme { get; set; }
    public CountryList? Country { get; set; }
    public CityList? City { get; set; }
    public AgeList? Age { get; set; }
    public GenderList? YourGender { get; set; }
    public GenderList? PartnerGender { get; set; }
    public LangList? Language { get; set; }
    public string? EmailConfirmToken { get; set; }
    public DateTimeOffset? EmailConfirmTokenExp { get; set; }
    
    public ICollection<QuickMessage> QuickMessages { get; set; } = new List<QuickMessage>();
    public ICollection<BlockedPerson> BlockerPeoples { get; set; } = new List<BlockedPerson>();
    public ICollection<BlockedPerson> BlockedPeoples { get; set; } = new List<BlockedPerson>();
    public ICollection<SingleChat> FirstPerson { get; set; } = new List<SingleChat>();
    public ICollection<SingleChat> SecondPerson { get; set; } = new List<SingleChat>();
    public ICollection<SingleMessage> SingleMessages { get; set; } = new List<SingleMessage>();
    public ICollection<Report> Reporter { get; set; } = new List<Report>();
    public ICollection<Report> Reported { get; set; } = new List<Report>();
    public ICollection<GroupMessage> GroupMessages { get; set; } = new List<GroupMessage>();
    public ICollection<BlockedGroupMember> BlockedGroupMembers { get; set; } = new List<BlockedGroupMember>();
    public GroupMember GroupMember { get; set; }
}