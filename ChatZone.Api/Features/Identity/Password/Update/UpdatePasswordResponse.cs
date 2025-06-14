namespace ChatZone.Features.Identity.Password.Update;

public class UpdatePasswordResponse
{
    public required string AccessToken { get; set; }
    public required string RefreshToken { get; set; }
}