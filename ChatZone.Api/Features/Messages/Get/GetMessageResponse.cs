namespace ChatZone.Features.Messages.Get;

public class GetMessageResponse
{
    public int IdChat { get; set; }
    public required List<MessageInfoDTO> Message { get; set; }
}