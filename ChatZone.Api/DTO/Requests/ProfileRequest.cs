namespace ChatZone.DTO.Requests;

public class ProfileRequest
{
    public required string Username { get; set; }
    public bool IsFindByProfile { get; set; }
}