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
            .HasKey(x => x.IdPerson);

        builder
            .Property(x => x.Role)
            .IsRequired();
        
        builder
            .Property(x => x.Username)
            .IsRequired()
            .HasMaxLength(20);

        builder
            .HasIndex(x => x.Username)
            .IsUnique();

        builder
            .Property(x => x.Email)
            .IsRequired()
            .HasMaxLength(50);
        
        builder
            .HasIndex(x => x.Email)
            .IsUnique();

        builder
            .Property(x => x.Password)
            .IsRequired()
            .HasMaxLength(50);

        builder
            .Property(x => x.Salt)
            .IsRequired();
        
        builder
            .Property(x => x.RefreshToken)
            .IsRequired();

        builder
            .Property(x => x.RefreshTokenExp)
            .IsRequired()
            .HasDefaultValueSql("GETUTCDATE()");

        builder
            .Property(x => x.LangMenu)
            .IsRequired()
            .HasDefaultValue(LangList.English);

        builder
            .Property(x => x.IsDarkTheme)
            .IsRequired()
            .HasDefaultValue(false);

        builder
            .Property(x => x.IsFindByProfile)
            .IsRequired()
            .HasDefaultValue(true);
        

        builder.ToTable(nameof(Person));
    }
}