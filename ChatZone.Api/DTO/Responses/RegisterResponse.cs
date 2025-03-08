namespace ChatZone.DTO.Responses;

public class RegisterResponse
{
    public required string AccessToken { get; set; }
    public required string RefreshToken { get; set; }
}