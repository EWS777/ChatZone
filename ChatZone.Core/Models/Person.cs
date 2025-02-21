using ChatZone.Core.Models.Enums;

namespace ChatZone.Core.Models;

public class Person
{
    public int PersonId { get; set; }
    public PersonRole Role { get; set; }
    public ICollection<QuickMessage> QuickMessages { get; set; } = new List<QuickMessage>();
    public required PersonData PersonData { get; set; }
    public required PersonSettings PersonSettings { get; set; }
    public ICollection<BlockedPeople> BlockerPeoples { get; set; } = new List<BlockedPeople>();
    public ICollection<BlockedPeople> BlockedPeoples { get; set; } = new List<BlockedPeople>();
    public ICollection<Report> Reporter { get; set; } = new List<Report>();
    public ICollection<Report> Reported { get; set; } = new List<Report>();
    public ICollection<Message> Messages { get; set; } = new List<Message>();
    public required ChatMember ChatMember { get; set; }
    public required IndividualChat FirstPerson { get; set; }
    public required IndividualChat SecondPerson { get; set; }

}