namespace ChatZone.Core.Models;

public class Country
{
    public int IdCountry { get; set; }
    public required string IsoCode { get; set; } //using just for Seeder
    public required string Name { get; set; }
    public ICollection<City> Cities { get; set; } = new List<City>();
}
