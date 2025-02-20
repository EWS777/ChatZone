namespace Chat.Models;

public class Chat
{
    public int ChatId { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    
    public ICollection<Report> Reports = new List<Report>();
    public ICollection<Message> Messages = new List<Message>();
    public GroupChat GroupChat { get; set; }
    public IndividualChat IndividualChat { get; set; }
}