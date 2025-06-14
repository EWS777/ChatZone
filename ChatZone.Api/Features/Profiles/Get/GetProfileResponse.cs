namespace ChatZone.Features.Profiles.Get;

public class GetProfileResponse
{
    public required string Username { get; set; }
    public required string Email { get; set; }
    public bool IsFindByProfile { get; set; }
}