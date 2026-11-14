using ChatZone.Core.Models.Enums;

namespace ChatZone.Core.Models;

public class SingleChat
{
    public int IdSingleChat { get; set; }
    public int IdFirstPerson { get; set; }
    public Person FirstPerson { get; set; }
    public int IdSecondPerson { get; set; }
    public Person SecondPerson { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset? FinishedAt { get; set; }
    
    public ThemeList? Theme { get; set; }
    public AgeList? Age { get; set; }
    public int? IdCountry { get; set; }
    public Country? Country { get; set; }
    public int? IdCity { get; set; }
    public City? City { get; set; }
    public int? IdLanguage { get; set; }
    public Language? Language { get; set; }

    public ICollection<SingleMessage> SingleMessages { get; set; } = new List<SingleMessage>();
}