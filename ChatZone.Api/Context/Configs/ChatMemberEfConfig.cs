using ChatZone.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ChatZone.Context.Configs;

public class ChatMemberEfConfig : IEntityTypeConfiguration<ChatMember>
{
    public void Configure(EntityTypeBuilder<ChatMember> builder)
    {
        builder
            .HasKey(x=> new {x.PersonMemberId, x.GroupChatId});

        builder
            .HasOne(x => x.Person)
            .WithOne(x => x.ChatMember)
            .HasForeignKey<ChatMember>(x => x.PersonMemberId)
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .HasOne(x => x.GroupChat)
            .WithMany(x => x.ChatMembers)
            .HasForeignKey(x => x.GroupChatId)
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .Property(x => x.IsAdmin)
            .IsRequired()
            .HasDefaultValue(false);

        builder
            .Property(x => x.JoinedAt)
            .IsRequired()
            .HasDefaultValueSql("GETUTCDATE()");
        
        builder.ToTable(nameof(ChatMember));
    }
}