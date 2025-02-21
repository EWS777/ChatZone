using ChatZone.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ChatZone.Context.Configs;

public class BlockedPeopleEfConfig : IEntityTypeConfiguration<BlockedPeople>
{
    public void Configure(EntityTypeBuilder<BlockedPeople> builder)
    {
        builder
            .HasKey(x => x.BlockerPersonId);

        builder
            .HasOne(x => x.Blocker)
            .WithMany(x => x.BlockerPeoples)
            .HasForeignKey(x => x.BlockerPersonId)
            .OnDelete(DeleteBehavior.NoAction);

        builder
            .HasOne(x => x.Blocked)
            .WithMany(x => x.BlockedPeoples)
            .HasForeignKey(x => x.BlockedPersonId)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.ToTable(nameof(BlockedPeople));
    }
}