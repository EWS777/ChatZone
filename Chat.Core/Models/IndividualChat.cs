namespace Chat.Core.Models;

public class IndividualChat : Chat
{
    public int IndividualChatId { get; set; }
    public Chat Chat { get; set; }

    public int FirstPersonId { get; set; }
    public Person FirstPerson { get; set; }
    public int SecondPersonId { get; set; }
    public Person SecondPerson { get; set; }
}