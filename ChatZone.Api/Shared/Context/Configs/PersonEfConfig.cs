using ChatZone.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ChatZone.Shared.Context.Configs;

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
            .HasMaxLength(30);

        builder
            .HasIndex(x => x.Username)
            .IsUnique();

        builder
            .Property(x => x.Email)
            .IsRequired()
            .HasMaxLength(254);
        
        builder
            .HasIndex(x => x.Email)
            .IsUnique();

        builder
            .Property(x => x.Password)
            .IsRequired()
            .HasMaxLength(64);

        builder
            .Property(x => x.Salt)
            .IsRequired();
        
        builder
            .Property(x => x.RefreshToken)
            .IsRequired();

        builder
            .Property(x => x.RefreshTokenExp)
            .IsRequired();

        builder
            .Property(x => x.LangMenu)
            .IsRequired();

        builder
            .Property(x => x.IsDarkTheme)
            .IsRequired()
            .HasDefaultValue(false);

        builder
            .HasIndex(x => x.RefreshToken)
            .IsUnique()
            .HasFilter("[RefreshToken] <> ''");

        builder
            .HasIndex(x => x.EmailConfirmToken)
            .IsUnique()
            .HasFilter("[EmailConfirmToken] IS NOT NULL");

        builder
            .HasOne(x => x.Country)
            .WithMany()
            .HasForeignKey(x => x.IdCountry)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasOne(x => x.City)
            .WithMany()
            .HasForeignKey(x => x.IdCity)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasOne(x => x.Language)
            .WithMany()
            .HasForeignKey(x => x.IdLanguage)
            .OnDelete(DeleteBehavior.Restrict);

        builder.ToTable(nameof(Person));
    }
}