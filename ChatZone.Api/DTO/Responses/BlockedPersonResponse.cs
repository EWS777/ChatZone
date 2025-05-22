namespace ChatZone.DTO.Responses;

public class BlockedPersonResponse
{
    public required string BlockedUsername { get; set; }
    public int IdBlockedPerson { get; set; }
    public DateTime CreatedAt { get; set; }
}