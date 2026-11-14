namespace ChatZone.Core.Models;

public class City
{
    public int IdCity { get; set; }
    public int IdCountry { get; set; }
    public Country Country { get; set; }
    public required string Name { get; set; } 
    public int Population { get; set; }
    public int GeoNameId { get; set; }
}
