namespace ChatZone.Features.Chats.Common.Get;

public class GetChatPersonInfoResponse
{
    public int IdPerson { get; set; }
    public int? IdGroup { get; set; }
    public bool? IsSingleChat { get; set; }
    public int? IdPartnerPerson { get; set; }
    public bool? IsSentMessage { get; set; }
    public string? PartnerUsername { get; set; }
    public ChatSearchInfo? Search { get; set; }
}

public class ChatSearchInfo
{
    public int? Theme { get; set; }
    public int? Country { get; set; }
    public int? City { get; set; }
    public int? Age { get; set; }
    public int? Language { get; set; }
}