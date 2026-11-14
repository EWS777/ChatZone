using ChatZone.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ChatZone.Shared.Context.Configs;

public class CountryEfConfig : IEntityTypeConfiguration<Country>
{
    public void Configure(EntityTypeBuilder<Country> builder)
    {
        builder.HasKey(x => x.IdCountry);

        builder
            .Property(x => x.IsoCode)
            .IsRequired()
            .HasMaxLength(2)
            .IsFixedLength();

        builder
            .HasIndex(x => x.IsoCode)
            .IsUnique();

        builder
            .Property(x => x.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.ToTable(nameof(Country));
    }
}
