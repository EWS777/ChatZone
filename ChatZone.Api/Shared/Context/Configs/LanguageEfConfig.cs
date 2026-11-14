using ChatZone.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ChatZone.Shared.Context.Configs;

public class LanguageEfConfig : IEntityTypeConfiguration<Language>
{
    public void Configure(EntityTypeBuilder<Language> builder)
    {
        builder.HasKey(x => x.IdLanguage);

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

        builder.ToTable(nameof(Language));
    }
}
