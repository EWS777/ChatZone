namespace ChatZone.Features.Identity.Authentication.Login;

public class LoginResponse
{
    public required string AccessToken { get; set; }
    public required string RefreshToken { get; set; }
}