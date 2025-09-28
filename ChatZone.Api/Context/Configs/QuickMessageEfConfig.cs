using ChatZone.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ChatZone.Context.Configs;

public class QuickMessageEfConfig : IEntityTypeConfiguration<QuickMessage>
{
    public void Configure(EntityTypeBuilder<QuickMessage> builder)
    {
        builder
            .HasKey(x => x.IdQuickMessage);

        builder
            .HasOne(x => x.Person)
            .WithMany(x => x.QuickMessages)
            .HasForeignKey(x => x.IdPerson)
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .Property(x => x.Message)
            .IsRequired()
            .HasMaxLength(50);
        
        builder.ToTable(nameof(QuickMessage));
    }
}