namespace ChatZone.Features.Identity.Authentication.Refresh;

public class RefreshResponse
{
    public required string AccessToken { get; set; }
    public required string RefreshToken { get; set; }
}