using ChatZone.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ChatZone.Shared.Context.Configs;

public class CityEfConfig : IEntityTypeConfiguration<City>
{
    public void Configure(EntityTypeBuilder<City> builder)
    {
        builder.HasKey(x => x.IdCity);

        builder
            .Property(x => x.Name)
            .IsRequired()
            .HasMaxLength(200)
            .UseCollation("Latin1_General_100_CI_AI"); // ingores case and special letters like polish

        builder
            .HasOne(x => x.Country)
            .WithMany(x => x.Cities)
            .HasForeignKey(x => x.IdCountry)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasIndex(x => x.GeoNameId)
            .IsUnique();
        
        builder
            .HasIndex(x => new { x.IdCountry, x.Name })
            .IsUnique();

        builder.ToTable(nameof(City));
    }
}
