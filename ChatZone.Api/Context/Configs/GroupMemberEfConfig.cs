using ChatZone.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ChatZone.Context.Configs;

public class GroupMemberEfConfig : IEntityTypeConfiguration<GroupMember>
{
    public void Configure(EntityTypeBuilder<GroupMember> builder)
    {
        builder.HasKey(x => new {x.IdChat, x.IdGroupMember});

        builder
            .HasOne(x => x.Person)
            .WithOne(x => x.GroupMember)
            .HasForeignKey<GroupMember>(x => x.IdGroupMember)
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .HasOne(x => x.GroupChat)
            .WithMany(x => x.ChatMembers)
            .HasForeignKey(x => x.IdChat)
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .Property(x => x.IsAdmin)
            .IsRequired()
            .HasDefaultValue(false);

        builder
            .Property(x => x.JoinedAt)
            .IsRequired();
        
        builder.ToTable(nameof(GroupMember));
    }
}