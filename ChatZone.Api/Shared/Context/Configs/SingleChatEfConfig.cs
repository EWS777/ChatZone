using ChatZone.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ChatZone.Shared.Context.Configs;

public class SingleChatEfConfig : IEntityTypeConfiguration<SingleChat>
{
    public void Configure(EntityTypeBuilder<SingleChat> builder)
    {
        builder
            .HasKey(x => x.IdSingleChat);

        builder
            .HasOne(x => x.FirstPerson)
            .WithMany(x => x.FirstPerson)
            .HasForeignKey(x => x.IdFirstPerson)
            .OnDelete(DeleteBehavior.Restrict);
        
        builder
            .HasOne(x => x.SecondPerson)
            .WithMany(x => x.SecondPerson)
            .HasForeignKey(x => x.IdSecondPerson)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .Property(x => x.CreatedAt)
            .IsRequired();

        builder
            .Property(x => x.FinishedAt)
            .IsRequired(false);
        
        builder.ToTable(nameof(SingleChat));
    }
}