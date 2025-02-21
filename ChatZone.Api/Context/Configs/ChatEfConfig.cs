using ChatZone.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ChatZone.Context.Configs;

public class ChatEfConfig : IEntityTypeConfiguration<Chat>
{
    public void Configure(EntityTypeBuilder<Chat> builder)
    {
        builder
            .HasKey(x => x.ChatId);

        builder
            .Property(x => x.CreatedAt)
            .HasDefaultValueSql("GETUTCDATE()")
            .IsRequired();

        builder
            .Property(x => x.ChatType)
            .IsRequired();
        
        builder.ToTable(nameof(Chat));
    }
}