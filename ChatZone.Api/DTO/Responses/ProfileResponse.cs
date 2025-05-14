namespace ChatZone.DTO.Responses;

public class ProfileResponse
{
    public required string Username { get; set; }
    public required string Email { get; set; }
    public bool IsFindByProfile { get; set; }
}