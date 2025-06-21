namespace ChatZone.Features.BlockedPersons.GetList;

public class GetBlockedPersonsResponse
{
    public required string BlockedUsername { get; set; }
    public int IdBlockedPerson { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
}