namespace ChatZone.Core.Models;

public class Language
{
    public int IdLanguage { get; set; }
    public required string IsoCode { get; set; }
    public required string Name { get; set; }
}
