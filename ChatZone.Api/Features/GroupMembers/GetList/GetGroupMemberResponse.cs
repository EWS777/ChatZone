namespace ChatZone.Features.GroupMembers.GetList;

public class GetGroupMemberResponse
{
    public int IdPerson { get; set; }
    public required string Username { get; set; }
}