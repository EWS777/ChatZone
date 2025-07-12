namespace ChatZone.Core.Models;

public class SingleChat
{
    public int IdSingleChat { get; set; }
    public int IdFirstPerson { get; set; }
    public Person FirstPerson { get; set; }
    public int IdSecondPerson { get; set; }
    public Person SecondPerson { get; set; }
    public DateTimeOffset CreatedAt { get; set; }

    public ICollection<SingleMessage> SingleMessages { get; set; } = new List<SingleMessage>();
}