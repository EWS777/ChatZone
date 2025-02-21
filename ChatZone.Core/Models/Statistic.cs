namespace ChatZone.Core.Models;

public class Statistic
{
    public int StatisticId { get; set; }
    public DateTimeOffset Day { get; set; }
    public int AllMember { get; set; }
    public int OnlineNow { get; set; }
    public int OnlineIndividual { get; set; }
    public int OnlineGroup { get; set; }
    public int OnlineToday { get; set; }
}