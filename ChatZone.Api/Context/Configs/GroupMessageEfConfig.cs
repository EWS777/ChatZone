using ChatZone.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ChatZone.Context.Configs;

public class GroupMessageEfConfig : IEntityTypeConfiguration<GroupMessage>
{
    public void Configure(EntityTypeBuilder<GroupMessage> builder)
    {
        builder.HasKey(x => x.IdGroupMessage);

        builder
            .HasOne(x => x.Person)
            .WithMany(x => x.GroupMessages)
            .HasForeignKey(x => x.IdSender)
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .HasOne(x => x.GroupChat)
            .WithMany(x => x.GroupMessages)
            .HasForeignKey(x => x.IdChat)
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .Property(x => x.Message)
            .HasMaxLength(250)
            .IsRequired();

        builder
            .Property(x => x.CreatedAt)
            .IsRequired()
            .HasDefaultValueSql("GETUTCDATE()");
        
        builder.ToTable(nameof(GroupMessage));
    }
}