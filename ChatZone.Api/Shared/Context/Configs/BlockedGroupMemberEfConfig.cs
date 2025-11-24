using ChatZone.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ChatZone.Shared.Context.Configs;

public class BlockedGroupMemberEfConfig : IEntityTypeConfiguration<BlockedGroupMember>
{
    public void Configure(EntityTypeBuilder<BlockedGroupMember> builder)
    {
        builder.HasKey(x => new { x.IdChat, x.IdBlockedPerson });

        builder
            .HasOne(x => x.Person)
            .WithMany(x => x.BlockedGroupMembers)
            .HasForeignKey(x => x.IdBlockedPerson)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasOne(x => x.GroupChat)
            .WithMany(x => x.BlockedGroupMembers)
            .HasForeignKey(x => x.IdChat)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .Property(x => x.BlockedAt)
            .IsRequired();
        
        builder.ToTable(nameof(BlockedGroupMember));
    }
}