using ChatZone.Core.Models.Enums;

namespace ChatZone.Core.Models;

public abstract class Chat
{
    public int ChatId { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public ChatType ChatType { get; set; }
    public ICollection<Message> Messages { get; set; } = new List<Message>();
}