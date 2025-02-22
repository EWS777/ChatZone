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
        
        builder.ToTable(nameof(PersonData));
    }
}