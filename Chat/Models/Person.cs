using Chat.Models.Enums;

namespace Chat.Models;

public class Person
{
    public int PersonId { get; set; }
    public PersonRole Role { get; set; }
    
    public int ChatId { get; set; }
    public Chat Chat { get; set; }
    
    public ICollection<QuickMessage> QuickMessages = new List<QuickMessage>();
    public PersonData PersonData { get; set; }
    public PersonSettings PersonSettings { get; set; }
    public ICollection<BlockedPeople> BlockerPeoples = new List<BlockedPeople>();
    public ICollection<BlockedPeople> BlockedPeoples = new List<BlockedPeople>();
    public ICollection<Report> Reporter = new List<Report>();
    public ICollection<Report> Reported = new List<Report>();
    public ICollection<Message> Messages = new List<Message>();
    public ChatMember ChatMember { get; set; }
    public IndividualChat FirstPerson { get; set; }
    public IndividualChat SecondPerson { get; set; }

}