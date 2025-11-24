using ChatZone.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ChatZone.Shared.Context.Configs;

public class BlockedPersonEfConfig : IEntityTypeConfiguration<BlockedPerson>
{
    public void Configure(EntityTypeBuilder<BlockedPerson> builder)
    {
        builder
            .HasKey(x => new { x.IdBlockerPerson, x.IdBlockedPerson });

        builder
            .HasOne(x => x.Blocker)
            .WithMany(x => x.BlockerPeoples)
            .HasForeignKey(x => x.IdBlockerPerson)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasOne(x => x.Blocked)
            .WithMany(x => x.BlockedPeoples)
            .HasForeignKey(x => x.IdBlockedPerson)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .Property(x => x.CreatedAt)
            .IsRequired();
        
        builder.ToTable(nameof(BlockedPerson));
    }
}