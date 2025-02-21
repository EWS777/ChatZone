using ChatZone.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ChatZone.Context.Configs;

public class PersonDataEfConfig : IEntityTypeConfiguration<PersonData>
{
    public void Configure(EntityTypeBuilder<PersonData> builder)
    {
        builder
            .HasKey(x => x.PersonDataId);

        builder
            .HasOne(x => x.Person)
            .WithOne(x => x.PersonData)
            .HasForeignKey<PersonData>(x => x.PersonDataId)
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .Property(x => x.Username)
            .IsRequired()
            .HasMaxLength(20);
        
        builder
            .Property(x => x.Email)
            .IsRequired()
            .HasMaxLength(50);

        builder
            .Property(x => x.Password)
            .IsRequired()
            .HasMaxLength(50);
        
        builder.ToTable(nameof(PersonData));
    }
}