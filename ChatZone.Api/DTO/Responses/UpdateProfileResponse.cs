namespace ChatZone.DTO.Responses;

public class UpdateProfileResponse
{
    public required string Username { get; set; }
    public bool IsFindByProfile { get; set; }
}