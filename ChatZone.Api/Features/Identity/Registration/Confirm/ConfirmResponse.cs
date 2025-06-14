namespace ChatZone.Features.Identity.Registration.Confirm;

public class ConfirmResponse
{
    public required string AccessToken { get; set; }
    public required string RefreshToken { get; set; }
}