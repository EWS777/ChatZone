namespace ChatZone.Features.Messages.Get;

public class MessageInfoDTO
{
    public int IdSender { get; set; }
    public required string Message { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
}