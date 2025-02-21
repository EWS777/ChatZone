using ChatZone.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ChatZone.Context.Configs;

public class PersonSettingsEfConfig : IEntityTypeConfiguration<PersonSettings>
{
    public void Configure(EntityTypeBuilder<PersonSettings> builder)
    {
        builder
            .HasKey(x => x.PersonSettingsId);

        builder
            .HasOne(x => x.Person)
            .WithOne(x => x.PersonSettings)
            .HasForeignKey<PersonSettings>(x => x.PersonSettingsId)
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .Property(x => x.IsDark)
            .IsRequired();

        builder
            .Property(x => x.IsFindByProfile)
            .IsRequired();
        
        builder.ToTable(nameof(PersonSettings));
    }
}