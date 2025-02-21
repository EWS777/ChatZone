using ChatZone.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ChatZone.Context.Configs;

public class QuickMessageEfConfig : IEntityTypeConfiguration<QuickMessage>
{
    public void Configure(EntityTypeBuilder<QuickMessage> builder)
    {
        builder
            .HasKey(x => x.PersonId);

        builder
            .HasOne(x => x.Person)
            .WithMany(x => x.QuickMessages)
            .HasForeignKey(x => x.PersonId)
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .Property(x => x.Message)
            .IsRequired();
        
        builder.ToTable(nameof(QuickMessage));
    }
}