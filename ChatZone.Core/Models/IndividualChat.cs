namespace ChatZone.Core.Models;

public class IndividualChat : Chat
{
    public int FirstPersonId { get; set; }
    public required Person FirstPerson { get; set; }
    public int SecondPersonId { get; set; }
    public required Person SecondPerson { get; set; }
}