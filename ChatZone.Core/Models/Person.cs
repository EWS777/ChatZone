using ChatZone.Core.Models.Enums;

namespace ChatZone.Core.Models;

public class Person
{
    public int PersonId { get; set; }
    public required PersonRole Role { get; set; }
    public required string Username { get; set; }
    public required string Email { get; set; }
    public required string Password { get; set; }
    public required string Salt { get; set; }
    public required string RefreshToken { get; set; }
    public required DateTime RefreshTokenExp { get; set; }
    public ICollection<QuickMessage> QuickMessages { get; set; } = new List<QuickMessage>();
    public PersonData PersonData { get; set; }
    public PersonSettings PersonSettings { get; set; }
    public ICollection<BlockedPeople> BlockerPeoples { get; set; } = new List<BlockedPeople>();
    public ICollection<BlockedPeople> BlockedPeoples { get; set; } = new List<BlockedPeople>();
    public ICollection<Report> Reporter { get; set; } = new List<Report>();
    public ICollection<Report> Reported { get; set; } = new List<Report>();
    public ICollection<Message> Messages { get; set; } = new List<Message>();
    public ChatMember ChatMember { get; set; }
    public IndividualChat FirstPerson { get; set; }
    public IndividualChat SecondPerson { get; set; }

}