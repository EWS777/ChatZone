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
    public CountryList? Country { get; set; }
    public CityList? City { get; set; }
    public LangList? Language { get; set; }

    public ICollection<SingleMessage> SingleMessages { get; set; } = new List<SingleMessage>();
}