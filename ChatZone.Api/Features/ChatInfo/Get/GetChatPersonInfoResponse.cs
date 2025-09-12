namespace ChatZone.Features.ChatInfo.Get;

public class GetChatPersonInfoResponse
{
    public int IdPerson { get; set; }
    public int? IdGroup { get; set; }
    public bool? IsSingleChat { get; set; }
    public int? IdPartnerPerson { get; set; }
    public bool? IsSentMessage { get; set; }
}