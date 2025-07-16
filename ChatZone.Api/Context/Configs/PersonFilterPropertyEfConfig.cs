using ChatZone.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ChatZone.Context.Configs;

public class PersonFilterPropertyEfConfig : IEntityTypeConfiguration<PersonFilterProperty>
{
    public void Configure(EntityTypeBuilder<PersonFilterProperty> builder)
    {
        builder.HasKey(x => x.IdPerson);

        builder.ToTable(nameof(PersonFilterProperty));
    }
}