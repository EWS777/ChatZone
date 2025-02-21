using ChatZone.Core.Models;
using ChatZone.Core.Models.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ChatZone.Context.Configs;

public class PersonEfConfig : IEntityTypeConfiguration<Person>
{
    public void Configure(EntityTypeBuilder<Person> builder)
    {
        builder
            .HasKey(x => x.PersonId);

        builder
            .Property(x => x.Role)
            .HasDefaultValue(PersonRole.User)
            .IsRequired();

        builder.ToTable(nameof(Person));
    }
}