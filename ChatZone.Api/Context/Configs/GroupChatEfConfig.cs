using ChatZone.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ChatZone.Context.Configs;

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
        
        builder.ToTable(nameof(GroupChat));
    }
}