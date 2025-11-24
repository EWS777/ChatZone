using ChatZone.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ChatZone.Shared.Context.Configs;

public class SingleMessageEfConfig : IEntityTypeConfiguration<SingleMessage>
{
    public void Configure(EntityTypeBuilder<SingleMessage> builder)
    {
        builder.HasKey(x => x.IdSingleMessage);

        builder
            .HasOne(x => x.Sender)
            .WithMany(x => x.SingleMessages)
            .HasForeignKey(x => x.IdSender)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasOne(x => x.SingleChat)
            .WithMany(x => x.SingleMessages)
            .HasForeignKey(x => x.IdChat)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .Property(x => x.Message)
            .IsRequired()
            .HasMaxLength(1024);

        builder
            .Property(x => x.CreatedAt)
            .IsRequired();
            
        builder.ToTable(nameof(SingleMessage));
    }
}