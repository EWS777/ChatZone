namespace ChatZone.Core.Models;

public class SingleChat
{
    public int IdSingleChat { get; set; }
    public int IdFirstPerson { get; set; }
    public required Person FirstPerson { get; set; }
    public int IdSecondPerson { get; set; }
    public required Person SecondPerson { get; set; }
    public DateTime CreatedAt { get; set; }

    public ICollection<SingleMessage> SingleMessages { get; set; } = new List<SingleMessage>();
}