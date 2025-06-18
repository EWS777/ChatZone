namespace ChatZone.Features.QuickMessages.Update;

public class UpdateQuickMessageResponse
{
    public int IdQuickMessage { get; set; }
    public required string Message { get; set; }
}