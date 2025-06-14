namespace ChatZone.Features.Profiles.Update;

public class UpdateProfileResponse
{
    public required string Username { get; set; }
    public bool IsFindByProfile { get; set; }
    public string? AccessToken { get; set; }
    public string? RefreshToken { get; set; }
}