using ChatZone.Core.Models;
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
            .IsRequired();
        
        builder
            .Property(x => x.Username)
            .HasMaxLength(20);

        builder
            .HasIndex(x => x.Username)
            .IsUnique();

        builder
            .Property(x => x.Email)
            .HasMaxLength(50);
        
        builder
            .HasIndex(x => x.Email)
            .IsUnique();

        builder
            .Property(x => x.Password)
            .HasMaxLength(50);

        builder
            .Property(x => x.Salt)
            .IsRequired();
        
        builder
            .Property(x => x.RefreshToken)
            .IsRequired();
        
        builder
            .Property(x => x.Salt)
            .IsRequired();

        builder.ToTable(nameof(Person));
    }
}