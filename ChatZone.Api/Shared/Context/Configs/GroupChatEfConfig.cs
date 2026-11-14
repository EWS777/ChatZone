using ChatZone.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ChatZone.Shared.Context.Configs;

public class GroupChatEfConfig : IEntityTypeConfiguration<GroupChat>
{
    public void Configure(EntityTypeBuilder<GroupChat> builder)
    {
        builder.HasKey(x => x.IdGroupChat);
        
        builder
            .Property(x => x.Title)
            .HasMaxLength(50)
            .IsRequired();

        builder
            .Property(x => x.UserCount)
            .IsRequired();
        
        builder
            .Property(x => x.CreatedAt)
            .IsRequired();
        
        builder
            .HasIndex(x => new { x.CreatedAt, x.IdGroupChat });

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

        builder.ToTable(nameof(GroupChat));
    }
}