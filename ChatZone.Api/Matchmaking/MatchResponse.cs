using ChatZone.Core.Models;

namespace ChatZone.Matchmaking;

public class MatchResponse
{
    public bool IsFound { get; set; }
    public MatchQueue? Person1 { get; set; }
    public MatchQueue? Person2 { get; set; }
    public int? IdGroup { get; set; }
}