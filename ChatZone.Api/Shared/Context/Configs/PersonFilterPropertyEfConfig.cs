using ChatZone.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ChatZone.Shared.Context.Configs;

public class PersonFilterPropertyEfConfig : IEntityTypeConfiguration<PersonFilterProperty>
{
    public void Configure(EntityTypeBuilder<PersonFilterProperty> builder)
    {
        builder.HasKey(x => x.IdPerson);

        builder.Property(x => x.IdPerson)
            .ValueGeneratedNever();

        builder.ToTable(nameof(PersonFilterProperty));
    }
}